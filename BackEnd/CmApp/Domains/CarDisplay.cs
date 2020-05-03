using CmApp.Entities;
using CodeMash.Models;

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
        public object CarImg { get; set; } = new object();
        [Field("summary")]
        public SummaryEntity Summary { get; set; } = new SummaryEntity();
        [Field("mainImageUrl")]
        public string MainImageUrl { get; set; }
    }
}
