using CmApp.Entities;

namespace CmApp.Domains
{
    public class Repair
    { 
        public string Name { get; set; }
        public double Price { get; set; }
        public CarEntity Car { get; set; }

        public Repair(string name, double price, CarEntity car)
        {
            Name = name;
            Price = price;
            Car = car;
        }
        public Repair()
        { }
    }
}
