using System.Collections.Generic;

namespace CmApp.Contracts.Entities
{
    public class CarMakesEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<CarModelEntity> Models { get; set; }

    }
}
