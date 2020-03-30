namespace CmApp.Domains
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Password2 { get; set; }

        public User(string email, string password1, string password2)
        {
            Email = email;
            Password = password1;
            Password2 = password2;
        }
    }
}
