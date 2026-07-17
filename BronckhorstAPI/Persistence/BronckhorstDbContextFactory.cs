using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistence;

public class BronckhorstDbContextFactory : IDesignTimeDbContextFactory<BronckhorstDbContext>
{
    public BronckhorstDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BronckhorstDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=Bronckhorst;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True");

        return new BronckhorstDbContext(optionsBuilder.Options);
    }
}
