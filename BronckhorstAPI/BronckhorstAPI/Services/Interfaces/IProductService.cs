using BronckhorstAPI.DTO;

namespace BronckhorstAPI.Services.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync();

    Task<List<ProductDto>> GetProductsByCategoryAsync(int categoryId);

    Task<ProductDto> GetProductByIdAsync(int id);
}
