using BronckhorstAPI.DTO;
using BronckhorstAPI.Mappers.Interfaces;
using BronckhorstAPI.Services.Interfaces;
using Domain.Entities;
using Persistence.Repositories.Interfaces;

namespace BronckhorstAPI.Services;

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

        return categoryEntities.Select(_categoryMapper.MapToDto).ToList();
    }
}
