using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Entities
{
    [Collection("makes")]
    public class CarMakes:Entity
    {
        [Field("name")]
        public string Name { get; set; }
    }
}
