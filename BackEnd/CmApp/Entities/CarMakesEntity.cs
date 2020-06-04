using CodeMash.Models;
using System.Collections.Generic;

namespace CmApp.Entities
{
    [Collection("makes")]
    public class CarMakesEntity : Entity
    {
        [Field("name")]
        public string Name { get; set; }
        [Field("models")]
        public List<Model> Models { get; set; } = new List<Model>();
    }

    public class Model
    {
        [Field("model")]
        public string Name { get; set; }
    }
}
