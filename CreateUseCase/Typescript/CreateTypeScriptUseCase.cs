namespace CreateUseCase
{
    class CreateTypeScriptUseCase : ICreateUseCase
    {
        #region LoadVariables
        private readonly string init_path;
        private GetData getData;
        private CreateFile createFile;
        #endregion
        public CreateTypeScriptUseCase(string init_path)
        {
            this.init_path = init_path;
            this.getData = new GetData();
        }

        public void Execute(string name, string data, string category)
        {
            this.createFile = new CreateFile(name, init_path);
            var generatorCommon = new GeneratorCommonFiles(this.createFile, this.getData, name, category);
            generatorCommon.CreatePresenter(name);
            generatorCommon.CreateHandler(name);
            generatorCommon.CreateAdapter(name);
            generatorCommon.CreateAction(name);
            generatorCommon.CreateCommand(name, data);
        }
    }
}