using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Domains
{
    public class ExchangeRateDetails
    {
        [JsonProperty("name")]
        public string Currency { get; set; }
    }
}
