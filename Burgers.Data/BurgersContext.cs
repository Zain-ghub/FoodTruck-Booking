using Microsoft.EntityFrameworkCore;
using Burgers.Models;
namespace Burgers.Data
{
    public class BurgersContext : DbContext
    {
        public BurgersContext(DbContextOptions<BurgersContext> options) : base(options)
        {
        }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
    }
    }

