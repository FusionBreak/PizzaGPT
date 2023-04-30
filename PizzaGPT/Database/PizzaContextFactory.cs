using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PizzaGPT.Database
{
    public class PizzaContextFactory : IDesignTimeDbContextFactory<PizzaContext>
    {
        public PizzaContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PizzaContext>();
            optionsBuilder.UseSqlite($"Data Source=pizza.db3");
            //Ensure database creation
            var context = new PizzaContext(optionsBuilder.Options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
