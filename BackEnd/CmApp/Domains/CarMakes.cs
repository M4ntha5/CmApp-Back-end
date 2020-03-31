using Newtonsoft.Json;
using System.Collections.Generic;

namespace CmApp.Domains
{
    public class CarMakes
    {
        [JsonProperty("Make_ID")]
        public string Id { get; set; }
        [JsonProperty("Make_Name")]
        public string Name { get; set; }

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
        public List<CarMakes> Cars { get; set; }

    }
}
