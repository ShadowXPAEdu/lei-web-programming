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
    }
}
