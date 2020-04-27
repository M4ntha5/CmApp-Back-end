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
        [Field("images")]
        public List<object> Images { get; set; } = new List<object>();
        [Field("main_image_url")]
        public string MainImageUrl { get; set; } = "";
        [Field("summary")]
        public List<SummaryEntity> Summary { get; set; } = new List<SummaryEntity>();

    }
}
