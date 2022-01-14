using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JCAirbnb.Models;

namespace JCAirbnb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Commodity> Commodities { get; set; }

        public DbSet<ReservationState> ReservationStates { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Property> Properties { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<PropertyCommodity> PropertyCommodities { get; set; }

        public DbSet<PropertyType> PropertyTypes { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<CheckListItem> CheckListItems { get; set; }

        public DbSet<CheckList> CheckLists { get; set; }

        public DbSet<Photo> Photos { get; set; }
    }
}
