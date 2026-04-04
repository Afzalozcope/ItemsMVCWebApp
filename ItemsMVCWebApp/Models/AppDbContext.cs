using System.Data.Entity;

namespace ItemsMVCWebApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("AppDbContext") { }

        public DbSet<Item> Items { get; set; }
    }
}