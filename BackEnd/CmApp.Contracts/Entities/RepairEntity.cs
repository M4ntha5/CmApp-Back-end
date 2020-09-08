namespace CmApp.Contracts.Entities
{
    public class RepairEntity
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public double Price { get; set; } = 0;
        public int Car { get; set; }
        public double Total { get; set; } = 0;
    }
}
