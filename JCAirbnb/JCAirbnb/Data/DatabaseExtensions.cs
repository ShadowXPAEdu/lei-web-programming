using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace JCAirbnb.Data
{
    public static class DatabaseExtensions
    {
        public static void InitializeDatabase(this IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher<IdentityUser>>();
            context.Database.EnsureCreated();
            if (!context.Roles.Any())
            {
                context.Roles.Add(new IdentityRole("Administrator")
                {
                    NormalizedName = "ADMINISTRATOR"
                });
                context.Roles.Add(new IdentityRole("Manager")
                {
                    NormalizedName = "MANAGER"
                });
                context.Roles.Add(new IdentityRole("Employee")
                {
                    NormalizedName = "EMPLOYEE"
                });
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                Guid guid = Guid.NewGuid();
                var user = new IdentityUser("Admin")
                {
                    Id = guid.ToString(),
                    NormalizedUserName = "ADMIN",
                    Email = "admin@admin.com",
                    NormalizedEmail = "ADMIN@ADMIN.COM",
                    EmailConfirmed = true
                };

                user.PasswordHash = passwordHasher.HashPassword(user, "Test1234!");
                user.SecurityStamp = Guid.NewGuid().ToString();

                context.Users.Add(user);
                context.SaveChanges();
            }

            if (!context.UserRoles.Any())
            {
                var userRole = new IdentityUserRole<string>
                {
                    RoleId = context.Roles.Where(r => r.Name == "Administrator").FirstOrDefault().Id,
                    UserId = context.Users.Where(u => u.UserName == "Admin").FirstOrDefault().Id
                };
                context.UserRoles.Add(userRole);
                context.SaveChanges();
            }
        }
    }
}
