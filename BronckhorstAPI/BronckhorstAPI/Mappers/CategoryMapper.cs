using BronckhorstAPI.DTO;
using BronckhorstAPI.Mappers.Interfaces;
using Domain.Entities;

namespace BronckhorstAPI.Mappers;

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
