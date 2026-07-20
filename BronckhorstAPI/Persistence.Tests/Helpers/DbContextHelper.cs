using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Tests.Helpers;

public static class DbContextHelper
{
    public static BronckhorstDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<BronckhorstDbContext>()
            .UseInMemoryDatabase("Bronckhorst")
            .Options;
        
        var context = new BronckhorstDbContext(options);
        
        AddProducts(context);
        
        return context;
    }

    private static void AddProducts(BronckhorstDbContext context)
    {
        context.Products.RemoveRange(context.Products);
        context.Products.AddRange(CreateProductEntities());
        context.SaveChanges();
    }
    
    public static List<Product> CreateProductEntities()
        => [
            new()
            {
                Id = 1,
                ArtikelCode = "ART001",
                Name = "Product 1",
                Description = "Description for Product 1",
                Price = 100
            },
            new()
            {
                Id = 2,
                ArtikelCode = "ART002",
                Name = "Product 2",
                Description = "Description for Product 2",
                Price = 200
            }
        ];
}