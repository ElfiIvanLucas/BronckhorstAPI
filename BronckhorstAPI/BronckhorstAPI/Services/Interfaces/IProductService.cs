using BronckhorstAPI.DTO;

namespace BronckhorstAPI.Services.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync();
    
    Task<ProductDto> GetProductByIdAsync(int id);
}