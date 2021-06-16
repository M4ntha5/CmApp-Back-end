using System;
using System.Collections;
using System.Collections.Generic;

namespace CmApp.Contracts.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string Country { get; set; }
        public DateTime? BornDate { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool Blocked { get; set; } = false;
        public bool Deleted { get; set; } = false;
        public string Currency { get; set; } = "EUR";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Car> Cars { get; set; }
        public ICollection<PasswordReset> PasswordResets { get; set; }
        public ICollection<UserRole> Roles { get; set; }

    }
}
