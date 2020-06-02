using CmApp.Domains;
using CodeMash.Models;
using System;
using System.Collections.Generic;

namespace CmApp.Entities
{
    [Collection("Cars")]
    public class CarEntity : Entity
    {
        [Field("user")]
        public string User { get; set; }
        [Field("make")]
        public string Make { get; set; }
        [Field("model")]
        public string Model { get; set; } = "";
        [Field("vin")]
        public string Vin { get; set; }
        [Field("created_at")]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        [Field("manufacture_date")]
        public DateTime ManufactureDate { get; set; } = DateTime.MinValue;
        [Field("series")]
        public string Series { get; set; } = "";
        [Field("body_type")]
        public string BodyType { get; set; } = "";
        [Field("steering")]
        public string Steering { get; set; } = "";
        [Field("engine")]
        public string Engine { get; set; } = "";
        [Field("displacement")]
        public double Displacement { get; set; } = 0;
        [Field("power")]
        public string Power { get; set; } = "";
        [Field("drive")]
        public string Drive { get; set; } = "";
        [Field("transmission")]
        public string Transmission { get; set; } = "";
        [Field("color")]
        public string Color { get; set; } = "";
        [Field("interior")]
        public string Interior { get; set; } = "";
        [Field("equipment")]
        public List<Equipment> Equipment { get; set; } = new List<Equipment>();
        [Field("urls")]
        public List<Urls> Urls { get; set; } = new List<Urls>();
        [Field("images")]
        public List<object> Images { get; set; } = new List<object>();
        public List<string> Base64images { get; set; } = new List<string>();
        public string MainImageUrl { get; set; } = "";

    }
}