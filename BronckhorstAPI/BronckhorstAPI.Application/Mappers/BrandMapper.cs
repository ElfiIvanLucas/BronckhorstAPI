using BronckhorstAPI.Application.DTO;
using BronckhorstAPI.Application.Mappers.Interfaces;
using BronckhorstAPI.Domain.Entities;

namespace BronckhorstAPI.Application.Mappers;

public class BrandMapper : IMapper<Brand, BrandDto>
{
    public BrandDto MapToDto(Brand entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name
        };
}
