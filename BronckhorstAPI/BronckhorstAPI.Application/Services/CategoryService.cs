using BronckhorstAPI.Application.DTO;
using BronckhorstAPI.Application.Mappers.Interfaces;
using BronckhorstAPI.Application.Repositories.Interfaces;
using BronckhorstAPI.Application.Services.Interfaces;
using BronckhorstAPI.Domain.Entities;

namespace BronckhorstAPI.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IMapper<Category, CategoryDto> _categoryMapper;
    private readonly IRepository<Category> _categoryRepository;

    public CategoryService(IRepository<Category> categoryRepository, IMapper<Category, CategoryDto> categoryMapper)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _categoryMapper = categoryMapper ?? throw new ArgumentNullException(nameof(categoryMapper));
    }

    public async Task<List<CategoryDto>> GetAllCategoriesAsync()
    {
        var categoryEntities = await _categoryRepository.GetAllAsync();

        return [.. categoryEntities.Select(_categoryMapper.MapToDto)];
    }
}
