using CodeMash.Models;

namespace LambdaFunction
{
    [Collection("Repairs")]
    public class RepairEntity : Entity
    {
        [Field("name")]
        public string Name { get; set; } = "";
        [Field("price")]
        public double Price { get; set; } = 0;
        [Field("car")]
        public string Car { get; set; } = "";
        public double Total { get; set; } = 0;
    }
}
