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
            context.Database.EnsureCreated();
            if (!context.Roles.Any())
            {
                context.Roles.Add(new IdentityRole("Administrator"));
                context.Roles.Add(new IdentityRole("Manager"));
                context.Roles.Add(new IdentityRole("Employee"));

                context.SaveChanges();
            }
        }
    }
}
