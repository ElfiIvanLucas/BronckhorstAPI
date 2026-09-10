using BronckhorstAPI.DTO;
using BronckhorstAPI.Mappers.Interfaces;
using Domain.Entities;

namespace BronckhorstAPI.Mappers;

public class ProductMapper : IMapper<Product, ProductDto>
{
    public ProductDto MapToDto(Product entity) =>
        new()
        {
            Id = entity.Id,
            ArtikelCode = entity.ArtikelCode,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            Image = entity.Image
        };
}
