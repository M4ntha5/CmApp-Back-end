namespace CmApp.Contracts.Entities
{
    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Make Make { get; set; }
    }
}
