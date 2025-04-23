using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EC2_2100212.Repositories.Data
{
    public class BagsDbContextFactory : IDesignTimeDbContextFactory<BagsDbContext>
    {
        public BagsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BagsDbContext>();


            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Bags;Trusted_Connection=True;MultipleActiveResultSets=True");

            return new BagsDbContext(optionsBuilder.Options);
        }
    }
}