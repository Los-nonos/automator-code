using System;
using System.Collections.Generic;
using System.IO;

namespace CreateUseCase
{
    class CreateTypeScriptUseCase : ICreateUseCase
    {
        #region LoadVariables
        private readonly string init_path;
        private string name_use_case;
        private string data;
        private string category;
        private GetData getData;
        private CreateFile createFile;

        const string Adapter = "Adapter";
        const string Command = "Command";
        const string Handler = "Handler";
        const string Action = "Action";
        const string Presenter = "Presenter";
        const string Schema = "Schema";
        #endregion
        public CreateTypeScriptUseCase(string init_path)
        {
            this.init_path = init_path;
            this.getData = new GetData();
        }

        public void Execute(string name, string data, string category)
        {
            this.name_use_case = name;
            this.category = category;
            this.data = data;
            this.createFile = new CreateFile(name, init_path);
            CreateCommand();
            CreateHandler();
            CreateAdapter();
            CreateAction();
        }

        private void CreateAdapter()
        {
            string path = this.createFile.CombinePath(string.Format("/src/API/Http/Adapters/{0}", this.category));
            this.createFile.VerificateFileOrCreate(path, this.createFile.CreateName(Adapter, "ts"));
        }

        private void CreateCommand()
        {
            string path = this.createFile.CombinePath("/src/Application/Commands");
            string name_file = this.createFile.CreateName(Command, "ts");
            this.createFile.VerificateFileOrCreate(path, name_file);
            path = Path.Combine(path, name_file);

            string name = this.createFile.CreateName(Command);

            List<DataDTO> info = this.getData.ClearData(this.data);
            List<string> _content = new List<string>() {
                "class " + name + " {",
                "\t" + MatchInfo.GetVariables (info),
                "\tconstructor(" + MatchInfo.GetParams (info) + ") {",
                "\t" + MatchInfo.GetConstructor (info),
                "\t}",
                "\t" + MatchInfo.GetFunctions (info),
                "}",
                "export default " + name + ";"
            };
            LoadFile(path, _content.ToArray());
        }

        private void LoadFile(string path, string[] content)
        {
            File.WriteAllLines(path, content);
        }

        private void CreateHandler()
        {
            string path = this.createFile.CombinePath(string.Format("/src/Application/Handlers/{0}", this.category));
            this.createFile.VerificateFileOrCreate(path, this.createFile.CreateName(Handler, "ts"));
        }

        private void CreateAction()
        {
            string path = this.createFile.CombinePath(string.Format("/src/API/Http/Actions/{0}", this.category));
            this.createFile.VerificateFileOrCreate(path, this.createFile.CreateName(Action, "ts"));
        }
    }
}