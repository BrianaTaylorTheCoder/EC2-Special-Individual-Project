using BNS2025.Repositories.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EC2_2100212.Repositories.Data
{
    public class BnsDbContextFactory : IDesignTimeDbContextFactory<BnsDbContext>
    {
        public BnsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BnsDbContext>();


            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=BNS2025;Trusted_Connection=True;MultipleActiveResultSets=True");

            return new BnsDbContext(optionsBuilder.Options);
        }
    }
}