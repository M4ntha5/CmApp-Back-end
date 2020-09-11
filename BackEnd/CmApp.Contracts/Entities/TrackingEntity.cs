using CmApp.Contracts.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmApp.Contracts.Entities
{
    public class TrackingEntity
    {
        [ForeignKey("Car")]
        public int ID { get; set; }
        public string Vin { get; set; } = "";
        public int Year { get; set; } = 0;
        public string Make { get; set; }
        public string Model { get; set; }
        public bool ShowImages { get; set; } = true;
        public string Title { get; set; } = "";
        public string State { get; set; } = "";
        public string Status { get; set; } = "";
        public DateTime DateReceived { get; set; } = DateTime.MinValue;
        public DateTime DateOrdered { get; set; } = DateTime.MinValue;
        public string Branch { get; set; } = "";
        public string ShippingLine { get; set; } = "";
        public string ContainerNumber { get; set; } = "";
        public string BookingNumber { get; set; } = "";
        public string Url { get; set; } = "";
        public string FinalPort { get; set; } = "";
        public DateTime DatePickedUp { get; set; } = DateTime.MinValue;
        public string Color { get; set; } = "";
        public string Damage { get; set; } = "";
        public string Condition { get; set; } = "";
        public string Keys { get; set; } = "";
        public string Running { get; set; } = "";
        public string Wheels { get; set; } = "";
        public string AirBag { get; set; } = "";
        public string Radio { get; set; } = "";
        

        public ICollection<UrlEntity> Images { get; set; }
        public virtual CarEntity Car { get; set; }

    }
}
