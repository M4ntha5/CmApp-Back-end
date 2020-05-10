using CodeMash.Models;

namespace CmApp.Domains
{
    public class CarStats
    {
        [Field("bought_price")]
        public double BoughtPrice { get; set; }
        [Field("sold_price")]
        public double SoldPrice { get; set; }
        [Field("sold_within")]
        public string SoldWithin { get; set; }
        [Field("car")]
        public string Car { get; set; }
        [Field("vin")]
        public string Vin { get; set; }
        [Field("user")]
        public string UserEmail { get; set; }
        [Field("spent")]
        public double AdditionallySpent { get; set; }
        [Field("profit")]
        public double Profit { get; set; }
    }
}
