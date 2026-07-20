using BronckhorstAPI.DTO;
using Domain.Entities;

namespace BronckhorstAPI.Tests.Helpers;

public static class ProductHelper
{
    public static ProductDto CreateProductDto()
        => new()
        {
            Id = 1,
            ArtikelCode = "ART001",
            Name = "Product 1",
            Description = "Description for Product 1",
            Price = 100
        };
    
    public static Product CreateProducEntity()
        => new()
        {
            Id = 1,
            ArtikelCode = "ART001",
            Name = "Product 1",
            Description = "Description for Product 1",
            Price = 100
        };
    
    public static List<Product> CreateProductEntities()
        => [
            new()
            {
                Id = 1,
                ArtikelCode = "ART001",
                Name = "Product 1",
                Description = "Description for Product 1",
                Price = 100
            }
        ];
    
    public static List<ProductDto> CreateProductDtos()
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