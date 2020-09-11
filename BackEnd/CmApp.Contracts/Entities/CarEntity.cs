using CmApp.Contracts.Domains;
using System;
using System.Collections.Generic;

namespace CmApp.Contracts.Entities
{
    public class CarEntity
    {
        public int ID { get; set; }
        public string Vin { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime ManufactureDate { get; set; } = DateTime.MinValue;
        public string Series { get; set; } = "";
        public string BodyType { get; set; } = "";
        public string Steering { get; set; } = "";
        public string Engine { get; set; } = "";
        public double Displacement { get; set; } = 0;
        public string Power { get; set; } = "";
        public string Drive { get; set; } = "";
        public string Transmission { get; set; } = "";
        public string Color { get; set; } = "";
        public string Interior { get; set; } = "";

        public ICollection<EquipmentEntity> Equipment { get; set; }
        public ICollection<RepairEntity> Repairs { get; set; }
        public ICollection<UrlEntity> Urls { get; set; }


        public virtual TrackingEntity Tracking { get; set; }
        public virtual SummaryEntity Summary { get; set; }
        public virtual ShippingEntity Shipping { get; set; }
        public virtual CarMakesEntity Make { get; set; }
        public virtual CarModelEntity Model { get; set; }


        public List<string> Base64images = new List<string>();
    }
}