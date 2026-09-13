using BronckhorstAPI.Application.DTO;

namespace BronckhorstAPI.Application.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllCategoriesAsync();
}
