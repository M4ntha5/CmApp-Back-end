using CodeMash.Models;
using System.Collections.Generic;

namespace CmApp.Entities
{
    [Collection("Users")]
    public class UserEntity : Entity
    {
        [Field("name")]
        public string Name { get; set; }
        [Field("email")]
        public string Email { get;set; }
        [Field("password")]
        public string Password { get; set; }
        [Field("blocked")]
        public bool Blocked { get; set; }
        [Field("cars")]
        public List<string> Cars { get; set; }
    }
}
