using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JCAirbnb.Models;
using Microsoft.AspNetCore.Identity;

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

        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Report>(e =>
            {
                e.HasMany(r => r.Photos).WithOne().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Property>(e =>
            {
                e.HasOne(p => p.Ratings).WithOne().OnDelete(DeleteBehavior.Cascade);
                e.HasOne(p => p.Divisions).WithOne().OnDelete(DeleteBehavior.Cascade);

                e.HasMany(p => p.Photos).WithOne().OnDelete(DeleteBehavior.Cascade);
                e.HasMany(p => p.Reviews).WithOne().OnDelete(DeleteBehavior.Cascade);
                e.HasMany(p => p.Commodities).WithOne().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Company>(e =>
            {
                e.HasMany(c => c.Employees).WithOne().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Client>(e =>
            {
                e.HasMany(c => c.Reviews).WithOne().OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
