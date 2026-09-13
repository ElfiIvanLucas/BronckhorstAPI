using BronckhorstAPI.Application.DTO;
using BronckhorstAPI.Application.Mappers.Interfaces;
using BronckhorstAPI.Domain.Entities;

namespace BronckhorstAPI.Application.Mappers;

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
