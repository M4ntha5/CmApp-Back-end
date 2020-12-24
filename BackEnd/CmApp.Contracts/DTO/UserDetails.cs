using System;

namespace CmApp.Contracts.DTO
{
    public class UserDetails
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; }
        public string Sex { get; set; } = "";
        public string Country { get; set; } = "";
        public DateTime BornDate { get; set; } = DateTime.MinValue;
        public string Currency { get; set; } = "";

    }
}
