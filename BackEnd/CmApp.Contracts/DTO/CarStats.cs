namespace CmApp.Contracts.DTO
{
    public class CarStats
    {
        public double BoughtPrice { get; set; }
        public double SoldPrice { get; set; }
        public string SoldWithin { get; set; }
        public string Car { get; set; }
        public string Vin { get; set; }
        public string UserEmail { get; set; }
        public double AdditionallySpent { get; set; }
        public double Profit { get; set; }
    }
}
