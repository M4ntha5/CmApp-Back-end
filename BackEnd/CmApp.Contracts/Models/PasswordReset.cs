using CmApp.Contracts.Models;
using System;

namespace CmApp.Contracts.Models
{
    public class PasswordReset
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ValidUntil { get; set; }

        public virtual User User { get; set; }
    }
}
