using CodeMash.Models;

namespace CmApp.Entities
{
    [Collection("makes")]
    public class CarMakes : Entity
    {
        [Field("name")]
        public string Name { get; set; }
    }
}
