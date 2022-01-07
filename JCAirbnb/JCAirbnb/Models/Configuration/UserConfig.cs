using Microsoft.AspNetCore.Identity;
using System;

namespace JCAirbnb.Models.Configuration
{
    public class UserConfig
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public IdentityUser GetUser(IPasswordHasher<IdentityUser> passwordHasher)
        {
            var user = new IdentityUser(UserName)
            {
                NormalizedUserName = UserName.ToUpper(),
                Email = Email,
                NormalizedEmail = Email.ToUpper(),
                EmailConfirmed = true
            };

            user.PasswordHash = passwordHasher.HashPassword(user, Password);
            user.SecurityStamp = Guid.NewGuid().ToString();

            return user;
        }
    }
}
