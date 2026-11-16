using Microsoft.EntityFrameworkCore;
using Burgers.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace Burgers.Data
{
    public class BurgersContext : IdentityDbContext<IdentityUser>
    {
        public BurgersContext(DbContextOptions<BurgersContext> options) : base(options)
        {
        }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
    }
    }

