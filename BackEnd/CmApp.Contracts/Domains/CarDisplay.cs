using CmApp.Contracts.Entities;

namespace CmApp.Contracts.Domains
{
    public class CarDisplay
    {
        public string Id { get; set; }
        public string User { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Vin { get; set; }
        public string CarImg { get; set; } = "";
        public SummaryEntity Summary { get; set; } = new SummaryEntity();
    }
}
