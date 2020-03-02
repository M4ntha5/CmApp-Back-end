﻿using CodeMash.Models;
using System;

namespace CmApp.Entities
{
    [Collection("Summary")]
    public class SummaryEntity : Entity
    {
        [Field("bought_price")]
        public double BoughtPrice { get; set; }
        [Field("sold_price")]
        public double SoldPrice { get; set; } = 0;
        [Field("sold_date")]
        public DateTime SoldDate { get; set; } = new DateTime();
        [Field("sold")]
        public bool Sold { get; set; } = false;
        [Field("car")]
        public string Car { get; set; }

    }
}
