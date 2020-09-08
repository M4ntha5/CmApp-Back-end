using System.Collections.Generic;

namespace CmApp.Contracts.Entities
{
    public class CarMakesEntity
    {
        public int ID { get; set; }
        public string Make { get; set; }
        public List<CarModelEntity> Models { get; set; } = new List<CarModelEntity>();
    }
}
