using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace DatabaseCodeFirst.Models
{
    public class BronckhorstDbContext : DbContext
    {
        public BronckhorstDbContext() 
        { 
        }

        public BronckhorstDbContext(DbContextOptions<BronckhorstDbContext> options) : base(options) { }

        public DbSet<Categorie> Categories { get; set; }

        public DbSet<Subcategorie> Subcategories { get; set; }
    }
}
