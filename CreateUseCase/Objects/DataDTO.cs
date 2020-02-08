using Newtonsoft.Json;

namespace CreateUseCase
{
    public class DataDTO
    {
        [JsonProperty("name")]
        public string type { get; set; }
        [JsonProperty("variableCommand")]
        public string Variable { get; set; }
        [JsonProperty("param")]
        public string Param { get; set; }
        [JsonProperty("function")]
        public string Function { get; set; }
        [JsonProperty("assignConstructor")]
        public string AssignConstructor { get; set; }
        [JsonProperty("schemaJoi")]
        public string SchemaJoi { get; set; }
    }
}