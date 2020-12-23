using CmApp.Contracts.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmApp.Contracts.Models
{
    public class Tracking
    {
        public int Id { get; set; }
        public string Vin { get; set; }
        public int? Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Title { get; set; }
        public string State { get; set; }
        public string Status { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DateOrdered { get; set; }
        public string Branch { get; set; }
        public string ShippingLine { get; set; }
        public string ContainerNumber { get; set; }
        public string BookingNumber { get; set; }
        public string FinalPort { get; set; }
        public DateTime? DatePickedUp { get; set; }
        public string Color { get; set; }
        public string Damage { get; set; }
        public string Condition { get; set; }
        public string Keys { get; set; }
        public string Running { get; set; }
        public string Wheels { get; set; }
        public string AirBag { get; set; }
        public string Radio { get; set; }
        public int CarId { get; set; }

        public virtual ICollection<TrackingImage> Images { get; set; }
        public virtual Car Car { get; set; }
    }
}
