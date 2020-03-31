using CodeMash.Models;

namespace CmApp.Entities
{
    [Collection("Users")]
    public class UserEntity : Entity
    {
        [Field("name")]
        public string FirstName { get; set; } = "";
        [Field("email")]
        public string Email { get;set; }
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
    }
}
