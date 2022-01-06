using JCAirbnb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
            AddRoles(context);
            AddUsers(context, passwordHasher);
            AddUserToRoles(context);
            AddReservationStates(context);
            AddCommodities(context);
        }

        private static void AddCommodities(ApplicationDbContext context)
        {
            if (!context.Commodities.Any())
            {
                var configCommodities = File.ReadAllText($@"Configuration\Commodities.json");
                var commodities = JsonConvert.DeserializeObject<List<Commodity>>(configCommodities);
                foreach (var commodity in commodities)
                {
                    commodity.Id = Guid.NewGuid().ToString();
                    context.Commodities.Add(commodity);
                }
                context.SaveChanges();
            }
        }

        private static void AddReservationStates(ApplicationDbContext context)
        {
            if (!context.ReservationStates.Any())
            {
                var configReservationStates = File.ReadAllText($@"Configuration\ReservationStates.json");
                var reservationStates = JsonConvert.DeserializeObject<List<ReservationState>>(configReservationStates);
                foreach (var reservationState in reservationStates)
                {
                    reservationState.Id = Guid.NewGuid().ToString();
                    context.ReservationStates.Add(reservationState);
                }
                context.SaveChanges();
            }
        }

        private static void AddUserToRoles(ApplicationDbContext context)
        {
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

        private static void AddUsers(ApplicationDbContext context, IPasswordHasher<IdentityUser> passwordHasher)
        {
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
        }

        private static void AddRoles(ApplicationDbContext context)
        {
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
        }
    }
}
