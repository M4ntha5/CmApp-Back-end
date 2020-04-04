using Newtonsoft.Json;
using System.Collections.Generic;

namespace CmApp.Domains
{
    public class CarModels
    {
        [JsonProperty("Make_ID")]
        public string Id { get; set; }
        [JsonProperty("Make_Name")]
        public string MakeName { get; set; }
        [JsonProperty("Model_ID")]
        public string ModelId { get; set; }
        [JsonProperty("Model_Name")]
        public string ModelName { get; set; }

    }
    public class CarMakesObject
    {
        [JsonProperty("Count")]
        public int Count { get; set; }
        [JsonProperty("Message")]
        public string Message { get; set; }
        [JsonProperty("SearchCriteria")]
        public string Search { get; set; }
        [JsonProperty("Results")]
        public List<CarModels> Cars { get; set; }

    }
}
