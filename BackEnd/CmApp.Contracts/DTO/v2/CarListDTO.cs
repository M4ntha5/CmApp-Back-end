using System;
using System.Collections.Generic;
using System.Text;

namespace CmApp.Contracts.DTO.v2
{
    public class CarListDTO
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Vin { get; set; }
        public string DefaultImage { get; set; }
        public decimal Total { get; set; }
        public bool Sold { get; set; }
        public string SoldWithin { get; set; }
        public decimal? Profit { get; set; }

    }
}
