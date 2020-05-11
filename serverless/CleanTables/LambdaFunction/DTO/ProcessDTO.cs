using Newtonsoft.Json;

namespace LambdaFunction
{
    public class ProcessDTO
    {
        [JsonProperty("apiKey")]
        public string APiKey { get; set; }
    }
}
