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
        public string CarName { get; set; }
        [Field("vin")]
        public string Vin { get; set; }
        [Field("user")]
        public string UserEmail { get; set; }
        [Field("spent")]
        public double Spent { get; set; }
        [Field("profit")]
        public double Profit { get; set; }
    }
}
