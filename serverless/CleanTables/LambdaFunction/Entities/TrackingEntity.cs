using CodeMash.Models;
using System;
using System.Collections.Generic;

namespace LambdaFunction
{
    [Collection("Tracking")]
    public class TrackingEntity : Entity
    {
        [Field("vin")]
        public string Vin { get; set; } = "";
        [Field("year")]
        public int Year { get; set; } = 0;
        [Field("make")]
        public string Make { get; set; } = "";
        [Field("model")]
        public string Model { get; set; } = "";
        [Field("show_images")]
        public bool ShowImages { get; set; } = true;
        [Field("title")]
        public string Title { get; set; } = "";
        [Field("state")]
        public string State { get; set; } = "";
        [Field("status")]
        public string Status { get; set; } = "";
        [Field("date_received")]
        public DateTime DateReceived { get; set; } = DateTime.MinValue;
        [Field("order_date")]
        public DateTime DateOrdered { get; set; } = DateTime.MinValue;
        [Field("branch")]
        public string Branch { get; set; } = "";
        [Field("shipping_line")]
        public string ShippingLine { get; set; } = "";
        [Field("container_number")]
        public string ContainerNumber { get; set; } = "";
        [Field("booking_number")]
        public string BookingNumber { get; set; } = "";
        [Field("url")]
        public string Url { get; set; } = "";
        [Field("final_port")]
        public string FinalPort { get; set; } = "";
        [Field("pick_up_date")]
        public DateTime DatePickedUp { get; set; } = DateTime.MinValue;
        [Field("color")]
        public string Color { get; set; } = "";
        [Field("damage")]
        public string Damage { get; set; } = "";
        [Field("condition")]
        public string Condition { get; set; } = "";
        [Field("keys")]
        public string Keys { get; set; } = "";
        [Field("running")]
        public string Running { get; set; } = "";
        [Field("wheels")]
        public string Wheels { get; set; } = "";
        [Field("air_bag")]
        public string AirBag { get; set; } = "";
        [Field("radio")]
        public string Radio { get; set; } = "";
        [Field("car")]
        public string Car { get; set; } = "";
        [Field("auction_photos")]
        public List<object> AuctionImages { get; set; } = new List<object>();
        public List<string> Base64images { get; set; } = new List<string>();

    }
}
