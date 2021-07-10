﻿namespace CmApp.Contracts.Models
{
    public class Repair
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsShipping { get; set; }
        public int CarId { get; set; }

        public virtual Car Car { get; set; }
    }
}
