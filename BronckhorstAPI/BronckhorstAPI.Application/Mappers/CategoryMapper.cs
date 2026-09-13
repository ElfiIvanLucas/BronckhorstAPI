using BronckhorstAPI.Application.DTO;
using BronckhorstAPI.Application.Mappers.Interfaces;
using BronckhorstAPI.Domain.Entities;

namespace BronckhorstAPI.Application.Mappers;

public class CategoryMapper : IMapper<Category, CategoryDto>
{
    public CategoryDto MapToDto(Category entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            TranslatedName = entity.TranslatedName,
            ParentId = entity.ParentId
        };
}
