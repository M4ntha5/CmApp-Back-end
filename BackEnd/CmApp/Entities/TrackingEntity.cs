using CodeMash.Models;
using System.Collections.Generic;

namespace CmApp.Entities
{
    [Collection("Tracking")]
    public class TrackingEntity : Entity
    {
        [Field("container_number")]
        public string ContainerNumber { get; set; }
        [Field("booking_number")]
        public string BookingNumber { get; set; }
        [Field("url")]
        public string Url { get; set; }
        [Field("car")]
        public string Car { get; set; }
        [Field("auction_photos")]
        public List<object> AuctionImages { get; set; }
        public List<string> Base64images { get; set; } = new List<string>();

    }
}
