using System;
using System.IO;

namespace CreateUseCase
{
    public class CreatePHPUseCase : ICreateUseCase
    {
        #region LoadVariables
        private readonly string init_path;
        private string name_use_case;

        const string Adapter = "Adapter";
        const string Command = "Command";
        const string Handler = "Handler";
        const string Controller = "Controller";
        #endregion

        public CreatePHPUseCase(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            this.init_path = path;
        }

        public void Execute(string name, string data, string category)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.name_use_case = name;
            CreateCommand();
            CreateHandler();
            CreateAdapter();
            CreateController();
        }

        private void CreateAdapter()
        {
            string path = this.CombinePath("/src/Application/API/Http/Adapters");
            VerificateFileOrCreate(path, this.CreateName(Adapter, "php"));
        }

        private void CreateCommand()
        {
            string path = this.CombinePath("/src/Domain/Commands");
            VerificateFileOrCreate(path, this.CreateName(Command, "php"));
        }

        private void CreateHandler()
        {
            string path = this.CombinePath("/src/Domain/Handlers");
            VerificateFileOrCreate(path, this.CreateName(Handler, "php"));
        }

        private void CreateController()
        {
            string path = this.CombinePath("/src/Application/Controllers");
            VerificateFileOrCreate(path, this.CreateName(Controller, "php"));
        }

        private void VerificateFileOrCreate(string path, string nameFile)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string path_file = Path.Combine(path, nameFile);

            File.Create(path_file);
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

        public bool VerificateUseCase(string category)
        {
            throw new NotImplementedException();
        }
    }
}