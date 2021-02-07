using System;
using System.Collections.Generic;
using System.Text;

namespace CmApp.Contracts.DTO.v2
{
    public class SummaryDTO
    {
        public decimal BoughtPrice { get; set; }
        public string BoughtPriceCurrency { get; set; }
        public string BaseCurrency { get; set; }
        public bool IsSold { get; set; } = false;
        public int CarId { get; set; }
    }
}
