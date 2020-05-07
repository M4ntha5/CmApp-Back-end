using CodeMash.Models;
using System;

namespace CmApp.Entities
{
    [Collection("Users")]
    public class UserEntity : Entity
    {
        [Field("first_name")]
        public string FirstName { get; set; } = "";
        [Field("last_name")]
        public string LastName { get; set; } = "";
        [Field("sex")]
        public string Sex { get; set; } = "";
        [Field("country")]
        public string Country { get; set; } = "";
        [Field("born_date")]
        public DateTime BornDate { get; set; } = new DateTime(1900, 01, 01, 0, 0, 0, DateTimeKind.Utc);
        [Field("email")]
        public string Email { get; set; }
        [Field("email_confirmed")]
        public bool EmailConfirmed { get; set; } = false;
        [Field("password")]
        public string Password { get; set; }
        [Field("salt")]
        public string Salt { get; set; }
        [Field("blocked")]
        public bool Blocked { get; set; } = false;
        [Field("deleted")]
        public bool Deleted { get; set; } = false;
        [Field("role")]
        public string Role { get; set; } = "user";
        [Field("currency")]
        public string Currency { get; set; } = "EUR";
    }
}
