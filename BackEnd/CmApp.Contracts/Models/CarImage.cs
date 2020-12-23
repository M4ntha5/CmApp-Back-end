using System;
using System.Collections.Generic;
using System.Text;

namespace CmApp.Contracts.Models
{
    public class CarImage
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int CarId { get; set; }

        public virtual Car Car { get; set; }
    }
}
