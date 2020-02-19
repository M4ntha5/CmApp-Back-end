using CodeMash.Models;
using System;
using System.Collections.Generic;

namespace CmApp.Entities
{
    [Collection("Cars")]
    public class CarEntity : Entity
    {
        [Field("make")]
        public string Make { get; set; }
        [Field("model")]
        public string Model { get; set; }
        [Field("vin")]
        public string Vin { get; set; }
        [Field("manufacture_date")]
        public DateTime ManufactureDate { get; set; }
        [Field("series")]
        public string Series { get; set; }
        [Field("body_type")]
        public string BodyType { get; set; }
        [Field("steering")]
        public string Steering { get; set; }
        [Field("engine")]
        public string Engine { get; set; }
        [Field("displacement")]
        public double Displacement { get; set; }
        [Field("power")]
        public string Power { get; set; }
        [Field("drive")]
        public string Drive { get; set; }
        [Field("transmission")]
        public string Transmission { get; set; }
        [Field("color")]
        public string Color { get; set; }
        [Field("interior")]
        public string Interior { get; set; }
        [Field("equipment")]
        public List<Equipment> Equipment { get; set; }
        [Field("images")]
        public List<object> Images { get; set; }
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
