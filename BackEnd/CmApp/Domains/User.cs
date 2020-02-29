using CmApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Domains
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }
        public bool Blocked { get; set; }
        public List<CarEntity> Cars { get; set; }

        public User(string name, string email, string password1, string password2, bool blocked, List<CarEntity> cars)
        {
            Name = name;
            Email = email;
            Password1 = password1;
            Password2 = password2;
            Blocked = blocked;
            Cars = cars;
        }
    }
}
