using System;
using System.Collections.Generic;

namespace CmApp.Contracts.DTO.v2
{
    public class CarDTO
    {
        public string Vin { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public string Series { get; set; }
        public string BodyType { get; set; }
        public string Steering { get; set; }
        public string FuelType { get; set; }
        public string Engine { get; set; }
        public decimal? Displacement { get; set; }
        public string Power { get; set; }
        public string Drive { get; set; }
        public string Transmission { get; set; }
        public string Color { get; set; }
        public string Interior { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public decimal BoughtPrice { get; set; }
        public string BoughtPriceCurrency { get; set; }


        public ICollection<EquipmentDTO> Equipment { get; set; }
        public ICollection<RepairDTO> Repairs { get; set; }
        public List<string> Images { get; set; }
    }
}
