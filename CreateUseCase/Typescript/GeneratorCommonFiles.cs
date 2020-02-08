namespace CreateUseCase
{
    internal class GeneratorCommonFiles
    {
        private CreateFile createFile;
        private GetData getData;
        private string category;
        private string name_use_case;
        

        public GeneratorCommonFiles(CreateFile createFile, GetData getData, string name, string category)
        {
            this.createFile = createFile;
            this.getData = getData;
            this.category = category;
            this.name_use_case = name;
        }

        public void CreateAdapter(string _case)
        {
            string path = this.createFile.CombinePath(Paths.AdapterPath, this.category);
            string fileName = this.createFile.CreateName(_case, EnumsFiles.Adapter, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string name = string.Format("{0}{1}", _case, EnumsFiles.Adapter);
            string name_command = string.Format("{0}{1}", _case, EnumsFiles.Command);
            string name_schema = string.Format("{0}{1}", name_use_case, EnumsFiles.Schema);

            string[] content = this.getData.GetContentAdapter(name, name_command, name_schema, this.category);

            this.createFile.LoadFile(path, fileName, content);
        }

        public void CreateAction(string _case)
        {
            string path = this.createFile.CombinePath(Paths.ActionPath, this.category);
            string fileName = this.createFile.CreateName(_case, EnumsFiles.Action, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string name = string.Format("{0}{1}", _case, EnumsFiles.Action);
            string name_handler = string.Format("{0}{1}", _case, EnumsFiles.Handler);
            string name_adapter = string.Format("{0}{1}", _case, EnumsFiles.Adapter);
            string[] content = this.getData.GetContentAction(name, name_adapter, name_handler, this.category);

            this.createFile.LoadFile(path, fileName, content);
        }

        public void CreatePresenter(string _case)
        {
            string path = this.createFile.CombinePath(Paths.PresenterPath, this.category);
            string fileName = this.createFile.CreateName(_case, EnumsFiles.Presenter, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string name = string.Format("{0}{1}", _case, EnumsFiles.Presenter);

            string[] content = this.getData.GetContentPresenter(name);

            this.createFile.LoadFile(path, fileName, content);
        }

        public void CreateHandler(string _case)
        {
            string path = this.createFile.CombinePath(Paths.HandlerPath, this.category);
            string fileName = this.createFile.CreateName(_case, EnumsFiles.Handler, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string name = string.Format("{0}{1}", _case, EnumsFiles.Handler);

            string[] content = this.getData.GetContentHandler(name, this.category, _case);

            this.createFile.LoadFile(path, fileName, content);
        }

        // TODO : Implement load variables
        public void CreateCommand(string _case, string data)
        {
            string path = this.createFile.CombinePath(Paths.CommandPath, this.category);
            string fileName = this.createFile.CreateName(_case, EnumsFiles.Command, "ts");
            this.createFile.VerificateFileOrCreate(path, fileName);

            string name = string.Format("{0}{1}", _case, EnumsFiles.Command);

            string[] content = this.getData.GetContentCommand(name, this.getData.ClearData(data));

            this.createFile.LoadFile(path, fileName, content);
        }
    }
}