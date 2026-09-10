using BronckhorstAPI.DTO;

namespace BronckhorstAPI.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllCategoriesAsync();
}
