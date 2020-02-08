
using System;

namespace CreateUseCase
{
    class CreateCRUDTypescrypt : ICreateUseCase
    {
        private string name_use_case;
        private string category;
        private string data;
        private string base_path;
        private CreateFile createFile;
        private GetData getData;

        public CreateCRUDTypescrypt(string base_path)
        {
            this.base_path = base_path;
        }

        public void Execute(string name, string data, string category)
        {
            this.name_use_case = name;
            this.data = data;
            this.category = category;

            this.getData = new GetData();
            this.createFile = new CreateFile(name, base_path);
            var generatorCode = new GeneratorCommonFiles(this.createFile, this.getData, name, category);

            string[] array = new string[] { "Create", "Edit", "Delete", "Find", "FindById" };
            foreach (var item in array)
            {
                var _data = string.Format("{0}{1}", item, name);
                generatorCode.CreateAction(_data);
                generatorCode.CreateAdapter(_data);
                generatorCode.CreateCommand(_data, data);
                generatorCode.CreateHandler(_data);
                generatorCode.CreatePresenter(_data);
            }
            this.CreateRouter(category);
            //this.CreateEntity(category);
            //this.CreateRepository(category);
            //this.CreateSchema(category);
        }

        private void CreateEntity(string category)
        {
            throw new NotImplementedException();
        }

        // TODO : Implement
        private void CreateRepository(string category)
        {
            throw new NotImplementedException();
        }  

        private void CreateRouter(string _case)
        {
            string path = this.createFile.CombinePath(Paths.RouterPath, string.Empty);
            string fileName = this.createFile.CreateName(_case.ToLower(), EnumsFiles.Router, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string[] content = this.getData.GetContentRouter(this.name_use_case, this.category);

            this.createFile.LoadFile(path, fileName, content);
        }

        // TODO: Implement
        private void CreateSchema(string _case)
        {
            string path = this.createFile.CombinePath(Paths.SchemaPath, this.category);
            string fileName = this.createFile.CreateName(name_use_case, EnumsFiles.Schema, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);
        }

        
    }
}