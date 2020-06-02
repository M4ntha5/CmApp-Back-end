using CmApp.Entities;
using CodeMash.Models;
using System.Collections.Generic;

namespace CmApp.Domains
{
    public class CarDisplay
    {
        [Field("_id")]
        public string Id { get; set; }
        [Field("user")]
        public string User { get; set; }
        [Field("make")]
        public string Make { get; set; }
        [Field("model")]
        public string Model { get; set; }
        [Field("vin")]
        public string Vin { get; set; }
        [Field("carImg")]
        public string CarImg { get; set; } = "";
        [Field("summary")]
        public SummaryEntity Summary { get; set; } = new SummaryEntity();
    }
}
