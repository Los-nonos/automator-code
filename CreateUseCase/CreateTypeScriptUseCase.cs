using System.IO;
using System;
using System.Collections.Generic;

namespace CreateUseCase
{
    class CreateTypeScriptUseCase : ICreateUseCase
    {
        #region LoadVariables
        private readonly string init_path;
        private string name_use_case;
        private string data;
        private GetData getData;

        const string Adapter = "Adapter";
        const string Command = "Command";
        const string Handler = "Handler";
        const string Controller = "Controller";
        #endregion
        public CreateTypeScriptUseCase(string init_path)
        {
            this.init_path = init_path;
            this.getData = new GetData();
        }

        public void Execute(string name, string data)
        {
            this.name_use_case = name;
            this.data = data;
            CreateCommand();
            CreateHandler();
            CreateAdapter();
            CreateController();
        }

        private void CreateAdapter()
        {
            string path = this.CombinePath("/src/Application/API/Http/Adapters");
            VerificateFileOrCreate(path, this.CreateName(Adapter, "ts"));
        }

        private void CreateCommand()
        {
            string path = this.CombinePath("/src/Domain/Commands");
            string name_file = this.CreateName(Command, "ts");
            VerificateFileOrCreate(path, name_file);
            path = Path.Combine(path, name_file);

            string name = CreateName(Command);

            List<DataDTO> info = this.getData.ClearData(this.data);
            string content = "class " + name + " {\n " + MatchInfo.GetVariables(info) + "\nconstructor(" + MatchInfo.GetParams(info) + ") {\n" + MatchInfo.GetConstructor(info) + "\n} " + MatchInfo.GetFunctions(info) + " } \n\n export default " + name + ";";
            List<string> _content = new List<string>(){
                "class " + name + " {",
                MatchInfo.GetVariables(info),
                "constructor("+MatchInfo.GetParams(info)+") {",
                MatchInfo.GetConstructor(info),
                "}",
                MatchInfo.GetFunctions(info),
                "}"
            };
            LoadFile(path, _content.ToArray());
        }

        private void LoadFile(string path, string[] content)
        {
            File.WriteAllLines(path, content);
        }

        private void CreateHandler()
        {
            string path = this.CombinePath("/src/Domain/Handlers");
            VerificateFileOrCreate(path, this.CreateName(Handler, "ts"));
        }

        private void CreateController()
        {
            string path = this.CombinePath("/src/Application/Controllers");
            VerificateFileOrCreate(path, this.CreateName(Controller, "ts"));
        }

        private void VerificateFileOrCreate(string path, string nameFile)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string path_file = Path.Combine(path, nameFile);

            var a = File.Create(path_file);
            a.Dispose();
            //return File.OpenWrite(path_file);
        }

        private string CreateName(string typeFile, string extension)
        {
            return string.Format("{0}{1}.{2}", this.name_use_case, typeFile, extension);
        }

        private string CreateName(string typeFile)
        {
            return string.Format("{0}{1}", this.name_use_case, typeFile);
        }

        private string CombinePath(string path)
        {
            return this.init_path + path;
        }
    }
}
