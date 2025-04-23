using BNS2025.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BNS2025.Repositories.Data
{
    public class BnsDbContext: IdentityDbContext<ApplicationUser>
    {

        public BnsDbContext(DbContextOptions options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Seed the database
            builder.Entity<Currency>().HasData(
                new Currency { Id = 1, Name = "Jamaican Dollar", ShortName = "JMD" },
                new Currency { Id = 2, Name = "United States Dollar", ShortName = "USD" },
                new Currency { Id = 3, Name = "Cayman Island Dollar", ShortName = "KYD" }
                );

        }

        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }


    }
} 
