using System.IO;
using System;

namespace CreateUseCase
{
    class CreateTypeScriptUseCase : ICreateUseCase
    {
        #region LoadVariables
        private readonly string init_path;
        private string name_use_case;

        const string Adapter = "Adapter";
        const string Command = "Command";
        const string Handler = "Handler";
        const string Controller = "Controller";
        #endregion
        public CreateTypeScriptUseCase(string init_path)
        {
            this.init_path = init_path;

        }

        public void Execute(string name)
        {
            this.name_use_case = name;
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
            LoadFile(path, Command);
        }

        private void LoadFile(string path, string typeFile)
        {
            var streamContent = File.ReadAllLines("../CreateUseCase/Data/command.ts.data");

            string data = null;
            foreach (var item in streamContent)
            {
                if (item.Split('|')[0] == "id")
                {
                    data = item;
                }
            }
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new Exception();
            }
            string name = CreateName(typeFile);

            string content = "class " + name + " {\n "+data.Split('|')[1]+"\nconstructor(" + data.Split('|')[2] + ") {\n" + data.Split('|')[3] + "\n} " + data.Split('|')[4] + " } \n\n export default " + name + ";";
            File.WriteAllText(path, content);
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
