using System;
using System.Collections.Generic;
using System.Text;

namespace CmApp.Contracts.Models
{
    public class TrackingImage
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int TrackingId { get; set; }

        public virtual Tracking Tracking { get; set; }
    }
}
