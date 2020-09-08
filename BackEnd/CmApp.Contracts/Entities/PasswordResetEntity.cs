using System;

namespace CmApp.Contracts.Entities
{
    public class PasswordResetEntity
    {
        public int ID { get; set; }
        public string Token { get; set; }
        public DateTime ValidUntil { get; set; }
        public UserEntity User { get; set; }
    }
}
