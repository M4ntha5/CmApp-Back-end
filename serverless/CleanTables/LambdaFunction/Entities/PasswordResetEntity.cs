using CodeMash.Models;
using System;

namespace LambdaFunction
{
    [Collection("passwords_reset")]
    public class PasswordResetEntity : Entity
    {
        [Field("token")]
        public string Token { get; set; }
        [Field("valid_until")]
        public DateTime ValidUntil { get; set; }
        [Field("user")]
        public string User { get; set; }
    }
}
