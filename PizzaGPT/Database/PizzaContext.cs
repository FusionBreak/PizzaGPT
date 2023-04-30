using Microsoft.EntityFrameworkCore;

namespace PizzaGPT.Database
{
    public class PizzaContext : DbContext
    {
        public PizzaContext(DbContextOptions options) : base(options)
        {
        }

        public PizzaContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source=pizza.db3");

        public DbSet<Order> Orders { get; set; }
    }
}
