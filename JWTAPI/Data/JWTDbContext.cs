using JWTAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTAPI.Data
{
    public class JWTDbContext : DbContext
    {
        public JWTDbContext(DbContextOptions<JWTDbContext> options): base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData
                (
                    new Product { Id = 1, Name = "Vel dolores et accumsan te", Price = 1200 },
                    new Product { Id = 2, Name = "No diam tincidunt duo nonumy", Price = 2300 },
                    new Product { Id = 3, Name = "Lorem rebum amet dolor", Price = 4300 }
                );

            modelBuilder.Entity<User>().HasData
                (
                    new User { Id = 1, FirstName = "John", LastName = "Marshal", UserName = "john", Password = "john1234" },
                    new User { Id = 2, FirstName = "Allen", LastName = "Green", UserName = "allen", Password = "allen1234" },
                    new User { Id = 3, FirstName = "Smith", LastName = "Wood", UserName = "smith", Password = "smith1234" }
                );
        }
    }
}
