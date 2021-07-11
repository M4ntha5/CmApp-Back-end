using CmApp.Contracts.Models;

namespace CmApp.Contracts.DTO
{
    public class CarDisplay
    {
        public string Id { get; set; }
        public string User { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Vin { get; set; }
        public string CarImg { get; set; } = "";
    }
}
