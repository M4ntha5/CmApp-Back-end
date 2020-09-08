using CmApp.Contracts.Domains;
using System;
using System.Collections.Generic;

namespace CmApp.Contracts.Entities
{
    public class CarEntity
    {
        public int ID { get; set; }
        public int User { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
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
        public List<Equipment> Equipment { get; set; } = new List<Equipment>();
        public List<Urls> Urls { get; set; } = new List<Urls>();
        public List<string> Base64images = new List<string>();
    }
}