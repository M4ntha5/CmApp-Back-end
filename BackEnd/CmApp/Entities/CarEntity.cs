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
        public DateTime ManufactureDate { get; set; }
        [Field("series")]
        public string Series { get; set; } = string.Empty;
        [Field("body_type")]
        public string BodyType { get; set; } = string.Empty;
        [Field("steering")]
        public string Steering { get; set; } = string.Empty;
        [Field("engine")]
        public string Engine { get; set; } = string.Empty;
        [Field("displacement")]
        public double Displacement { get; set; } = 0;
        [Field("power")]
        public string Power { get; set; } = string.Empty;
        [Field("drive")]
        public string Drive { get; set; } = string.Empty;
        [Field("transmission")]
        public string Transmission { get; set; } = string.Empty;
        [Field("color")]
        public string Color { get; set; } = string.Empty;
        [Field("interior")]
        public string Interior { get; set; } = string.Empty;
        [Field("equipment")]
        public List<Equipment> Equipment { get; set; } = new List<Equipment>();
        [Field("images")]
        public List<object> Images { get; set; } = new List<object>();
        public List<string> Base64images { get; set; } = new List<string>();
        public string MainImgUrl { get; set; } = "";

    }
    //nested form
    public class Equipment
    {
        [Field("code")]
        public string Code { get; set; }
        [Field("name")]
        public string Name { get; set; }
    }
}
