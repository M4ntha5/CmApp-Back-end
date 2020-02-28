using CodeMash.Models;
using System;
using System.Collections.Generic;


namespace CmApp.Entities
{
    [Collection("Summary")]
    public class SummaryEntity : Entity
    {
        [Field("bought_price")]
        public double BoughtPrice { get; set; }
        [Field("sold_price")]
        public double SoldPrice { get; set; }
        [Field("sold_date")]
        public DateTime SoldDate { get; set; }
        [Field("sold")]
        public bool Sold { get; set; }
        [Field("car")]
        public string Car { get; set; }
        [Field("transportation")]
        public List<object> TransportationFile { get; set; }

    }
}
