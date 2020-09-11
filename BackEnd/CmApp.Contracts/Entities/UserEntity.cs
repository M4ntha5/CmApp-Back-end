using System;
using System.Collections;
using System.Collections.Generic;

namespace CmApp.Contracts.Entities
{
    public class UserEntity
    {
        public int ID { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Sex { get; set; } = "";
        public string Country { get; set; } = "";
        public DateTime BornDate { get; set; } = new DateTime(1900, 01, 01, 0, 0, 0, DateTimeKind.Utc);
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool Blocked { get; set; } = false;
        public bool Deleted { get; set; } = false;
        public string Role { get; set; } = "user";
        public string Currency { get; set; } = "EUR";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<CarEntity> Cars { get; set; }
        public ICollection<PasswordResetEntity> PasswordResets { get; set; }

    }
}
