using Microsoft.EntityFrameworkCore;
using Persistence;

namespace BronckhorstAPI.Tests.Helpers;

public static class DbContextHelper
{
    public static BronckhorstDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<BronckhorstDbContext>()
            .UseInMemoryDatabase(databaseName: "Bronckhorst")
            .Options;

        return new BronckhorstDbContext(options);
    }
}