using CodeMash.Models;
using System;

namespace CmApp.Entities
{
    [Collection("Summary")]
    public class SummaryEntity : Entity
    {
        [Field("bought_price")]
        public double BoughtPrice { get; set; } = 0;
        [Field("sold_price")]
        public double SoldPrice { get; set; } = 0;
        [Field("sold_date")]
        public DateTime SoldDate { get; set; } = new DateTime(2000, 01, 01, 0, 0, 0, DateTimeKind.Utc);
        [Field("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Field("sold_within")]
        public string SoldWithin { get; set; } = "";
        [Field("sold")]
        public bool Sold { get; set; } = false;
        [Field("total")]
        public double Total { get; set; } = 0;
        [Field("car")]
        public string Car { get; set; }
        public string BaseCurrency { get; set; } = "";
        public string SelectedCurrency { get; set; } = "";
        [Field("profit")]
        public double Profit { get; set; }

    }
}
