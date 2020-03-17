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
        public DateTime SoldDate { get; set; } = new DateTime(1900,01,01);
        [Field("sold")]
        public bool Sold { get; set; } = false;
        [Field("total")]
        public double Total { get; set; } = 0;
        [Field("car")]
        public string Car { get; set; }

    }
}
