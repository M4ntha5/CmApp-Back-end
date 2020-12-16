using System;
using System.Collections.Generic;
using System.Text;

namespace CmApp.Contracts.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual Car Car { get; set; }
    }
}
