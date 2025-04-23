using EC2_2100212.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EC2_2100212.Repositories.Data
{
    public class BagsDbContext : IdentityDbContext<ApplicationUser>
    {
        public BagsDbContext(DbContextOptions<BagsDbContext> options) :base(options)
        {
            
        }

        public DbSet<Bag> Bags { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BagMaterial> BagMaterials { get; set; }
        public DbSet<BagCategory> BagCategories { get; set; }
        public DbSet<Order> Orders { get; set; }

    }
}
