using BronckhorstAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BronckhorstAPI.Tests.Helpers;

public static class DbContextHelper
{
    public static BronckhorstDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<BronckhorstDbContext>()
            .UseInMemoryDatabase("Bronckhorst")
            .Options;

        return new BronckhorstDbContext(options);
    }
}