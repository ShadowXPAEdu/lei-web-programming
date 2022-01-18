using JCAirbnb.Models;
using JCAirbnb.Models.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            Random rand = new();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher<IdentityUser>>();
            context.Database.EnsureCreated();
            AddRoles(context);
            var rolesByName = AddUsers(context, passwordHasher, rand);
            AddReservationStates(context);
            AddCommodities(context);
            AddPropertyTypes(context);
            AddProperties(rand, context, rolesByName);
            AddCheckLists(rand, context, rolesByName);
            AddReviews(rand, context);

            AddReservations(rand, context);
        }

        private static void AddReservations(Random rand, ApplicationDbContext context)
        {
            if (!context.Reservations.Any())
            {
                var properties = context.Properties.ToList();
                var reservationStates = context.ReservationStates.ToList();
                var users = context.Users.ToList();
                var checkLists = context.CheckLists.ToList();

                var config = File.ReadAllText($@"Configuration\Comments.json");
                var obj = JsonConvert.DeserializeObject<List<string>>(config);

                config = File.ReadAllText($@"Configuration\ReportPhotos.json");
                var photos = JsonConvert.DeserializeObject<List<Photo>>(config);

                var numReservations = rand.Next(50);

                for (int i = 0; i < numReservations; i++)
                {
                    var reservation = new Reservation()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CheckIn = DateTime.UtcNow.AddDays(-(rand.Next(4, 10) + rand.Next(30))),
                        CheckOut = DateTime.UtcNow.AddDays(-rand.Next(30)),
                        Property = properties[rand.Next(properties.Count)],
                        ReservationState = reservationStates[rand.Next(reservationStates.Count)],
                        User = users[rand.Next(users.Count)]
                    };

                    if (rand.Next(10) <= 5)
                        reservation.ReservationCheckList = checkLists[rand.Next(checkLists.Count)];
                    if (rand.Next(10) <= 5)
                        reservation.DeliveryCheckList = checkLists[rand.Next(checkLists.Count)];

                    if (reservation.ReservationState.Title == "Verifying")
                    {
                        reservation.Report = new Report()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Observations = obj[rand.Next(obj.Count)],
                            Photos = new()
                        };
                        var numPhotos = rand.Next(3);
                        for (int j = 0; j < numPhotos; j++)
                        {
                            var photo = photos[rand.Next(photos.Count)];
                            reservation.Report.Photos.Add(photo);
                        }
                    }
                    context.Reservations.Add(reservation);
                }

                context.SaveChanges();
            }
        }

        private static void AddReviews(Random rand, ApplicationDbContext context)
        {
            var users = context.Users.ToList();

            var config = File.ReadAllText($@"Configuration\Comments.json");
            var obj = JsonConvert.DeserializeObject<List<string>>(config);

            var properties = context.Properties.Include(p => p.Reviews).ToList();
            foreach (var p in properties)
            {
                if (p.Reviews.Count == 0)
                {
                    var numberOfReviews = rand.Next((obj.Count / properties.Count) + 2);

                    for (int i = 0; i < numberOfReviews; i++)
                    {
                        var review = new Review()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Text = obj[rand.Next(obj.Count)],
                            User = users[rand.Next(users.Count)],
                            Date = DateTime.UtcNow.AddDays(-rand.Next(90))
                        };

                        p.Reviews.Add(review);
                    }
                    context.SaveChanges();
                }
            }

            var clients = context.Clients.Include(c => c.Reviews).ToList();
            foreach (var c in clients)
            {
                if (c.Reviews.Count == 0)
                {
                    var numberOfReviews = rand.Next(5);

                    for (int i = 0; i < numberOfReviews; i++)
                    {
                        var review = new Review()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Text = obj[rand.Next(obj.Count)],
                            User = users[rand.Next(users.Count)],
                            Date = DateTime.UtcNow.AddDays(-rand.Next(90))
                        };

                        c.Reviews.Add(review);
                    }
                    context.SaveChanges();
                }
            }
        }

        private static void AddCheckLists(Random rand, ApplicationDbContext context, Dictionary<string, string> rolesByName)
        {
            if (!context.CheckLists.Any() && !context.CheckListItems.Any())
            {
                var managers = context.UserRoles.Where(ur => ur.RoleId == rolesByName["Manager"]).ToList();

                var config = File.ReadAllText($@"Configuration\CheckLists.json");
                var obj = JsonConvert.DeserializeObject<CheckListsConfig>(config);
                int i = 0;

                foreach (var cl in obj.CheckLists)
                {
                    var next = rand.Next(managers.Count);
                    var company = managers[next].UserId;
                    cl.Company = context.Companies.Find(company);
                    context.CheckLists.Add(cl);

                    for (int j = 0; j < 3 && i < obj.CheckListItems.Count; j++, i++)
                    {
                        var cli = obj.CheckListItems[i];
                        cli.CheckList = cl;
                        context.CheckListItems.Add(cli);
                    }
                }
                context.SaveChanges();
            }
        }

        private static void AddProperties(Random rand, ApplicationDbContext context, Dictionary<string, string> rolesByName)
        {
            if (!context.Properties.Any())
            {
                var managers = context.UserRoles.Where(ur => ur.RoleId == rolesByName["Manager"]).ToList();

                var commodities = context.Commodities.ToList();

                var config = File.ReadAllText($@"Configuration\Properties.json");
                var obj = JsonConvert.DeserializeObject<List<PropertiesConfig>>(config);
                foreach (var o in obj)
                {
                    var propId = Guid.NewGuid().ToString();
                    o.Divisions.Id = propId;
                    o.Ratings.Id = propId;

                    var propType = context.PropertyTypes.FirstOrDefault(pt => pt.Title == o.PropertyType);
                    var next = rand.Next(managers.Count);
                    var manager = context.Users.FirstOrDefault(m => m.Id == managers[next].UserId);

                    Property p = new()
                    {
                        Id = propId,
                        Title = o.Title,
                        Description = o.Description,
                        BasePrice = o.BasePrice,
                        Price = o.Price,
                        Location = o.Location,
                        Divisions = o.Divisions,
                        Ratings = o.Ratings,
                        PropertyType = propType,
                        Manager = manager,
                        Photos = new(),
                        Commodities = new(),
                        Reviews = new(),
                    };

                    foreach (var photo in o.Photos)
                    {
                        p.Photos.Add(photo);
                    }

                    var numCommodities = rand.Next(10);

                    for (int i = 0; i < numCommodities; i++)
                    {
                        var propCommodity = new PropertyCommodity()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Commodity = commodities[rand.Next(commodities.Count)],
                            Included = rand.Next(10) <= 5
                        };

                        p.Commodities.Add(propCommodity);
                    }

                    context.Properties.Add(p);
                }
                context.SaveChanges();
            }
        }

        private static void AddPropertyTypes(ApplicationDbContext context)
        {
            if (!context.PropertyTypes.Any())
            {
                var configPropertyTypes = File.ReadAllText($@"Configuration\PropertyType.json");
                var propertyTypes = JsonConvert.DeserializeObject<List<PropertyType>>(configPropertyTypes);
                foreach (var propertyType in propertyTypes)
                {
                    context.PropertyTypes.Add(propertyType);
                }
                context.SaveChanges();
            }
        }

        private static void AddCommodities(ApplicationDbContext context)
        {
            if (!context.Commodities.Any())
            {
                var configCommodities = File.ReadAllText($@"Configuration\Commodities.json");
                var commodities = JsonConvert.DeserializeObject<List<Commodity>>(configCommodities);
                foreach (var commodity in commodities)
                {
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
                    context.ReservationStates.Add(reservationState);
                }
                context.SaveChanges();
            }
        }

        private static Dictionary<string, string> AddUsers(ApplicationDbContext context, IPasswordHasher<IdentityUser> passwordHasher, Random rand)
        {
            Dictionary<string, string> rolesByName = new();
            foreach (var role in context.Roles)
            {
                rolesByName.Add(role.Name, role.Id);
            }

            if (!context.Users.Any())
            {
                var configUsers = File.ReadAllText($@"Configuration\Users.json");
                var users = JsonConvert.DeserializeObject<List<UserConfig>>(configUsers);


                foreach (var u in users)
                {
                    var user = u.GetUser(passwordHasher);
                    context.Users.Add(user);
                    context.Clients.Add(new Client
                    {
                        Id = user.Id,
                        Reviews = new()
                    });
                    context.SaveChanges();

                    if (!string.IsNullOrWhiteSpace(u.Role))
                        context.UserRoles.Add(new()
                        {
                            UserId = user.Id,
                            RoleId = rolesByName[u.Role]
                        });

                    AddUserToRole(context, rand, rolesByName, u, user);
                    context.SaveChanges();
                }
            }

            return rolesByName;
        }

        private static void AddUserToRole(ApplicationDbContext context, Random rand, Dictionary<string, string> rolesByName, UserConfig u, IdentityUser user)
        {
            switch (u.Role)
            {
                case "Employee":
                    {
                        var managers = context.UserRoles.Where(ur => ur.RoleId == rolesByName["Manager"]).ToList();
                        // Add to the employee list
                        var next = rand.Next(managers.Count);
                        var company = context.Companies.Include(c => c.Employees).FirstOrDefault(m => m.Id == managers[next].UserId);
                        Employee employee = new()
                        {
                            User = user
                        };
                        company.Employees.Add(employee);
                        context.Employees.Add(employee);
                    }
                    break;
                case "Manager":
                    {
                        // Create a company
                        context.Companies.Add(new()
                        {
                            Id = user.Id,
                            Manager = user
                        });
                    }
                    break;
                default:
                    break;
            }
        }

        private static void AddRoles(ApplicationDbContext context)
        {
            if (!context.Roles.Any())
            {
                var configRoles = File.ReadAllText($@"Configuration\Roles.json");
                var roles = JsonConvert.DeserializeObject<List<IdentityRole>>(configRoles);
                foreach (var role in roles)
                {
                    context.Roles.Add(role);
                }
                context.SaveChanges();
            }
        }
    }
}
