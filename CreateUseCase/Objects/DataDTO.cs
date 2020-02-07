namespace CreateUseCase
{
    public class DataDTO
    {
        private string type, variable, param, function, varConstructor;
        public DataDTO(string type, string variable, string param, string varConstructor, string function)
        {
            this.type = type;
            this.variable = variable;
            this.varConstructor = varConstructor;
            this.param = param;
            this.function = function;
        }

        public string GetTypeData()
        {
            return this.type;
        }

        public string GetVariable()
        {
            return this.variable;
        }

        public string GetParam()
        {
            return this.param;
        }

        public string GetFunction()
        {
            return this.function;
        }

        public string GetConstructor()
        {
            return this.varConstructor;
        }
    }
}