using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Domains
{
    public class ExchangeRate
    {
        [JsonProperty("rates")]
        public Dictionary<string, string> Rates { get; set; }
        [JsonProperty("base")]
        public string Base { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}
