using System;
using System.Collections.Generic;
using System.Text;

namespace CmApp.Contracts.DTO.v2
{
    public class RepairDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsShipping { get; set; }
    }
}
