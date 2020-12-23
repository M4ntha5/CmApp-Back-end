using CmApp.Contracts.Domains;
using CmApp.Contracts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CmApp.Contracts.Models
{
    public class Car
    {
        public int Id { get; set; }
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
        public int UserId { get; set; }
        public int TrackingId { get; set; }
        public int ShippingId { get; set; }
        public int SummaryId { get; set; }


        public virtual Make Make { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Equipment> Equipment { get; set; }
        public virtual ICollection<Repair> Repairs { get; set; }
        public virtual ICollection<CarImage> Images { get; set; }
        public virtual Tracking Tracking { get; set; }
        public virtual Shipping Shipping { get; set; }
        public virtual Summary Summary { get; set; }
    }
}