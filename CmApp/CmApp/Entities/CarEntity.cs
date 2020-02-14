using CodeMash.Models;
using System.Collections.Generic;

namespace CmApp.Entities
{
    [Collection("Cars")]
    public class CarEntity : Entity
    {
        [Field("parameters")]
        public List<Parameter> Parameters { get; set; }
        [Field("equipment")]
        public List<Equipment> Equipment { get; set; }
    }
    //nested form
    public class Parameter
    {
        [Field("type")]
        public string Type { get; set; }
        [Field("name")]
        public string Name { get; set; }
    }
    //nested form
    public class Equipment
    {
        [Field("type")]
        public string Type { get; set; }
        [Field("name")]
        public string Name { get; set; }
    }
}
