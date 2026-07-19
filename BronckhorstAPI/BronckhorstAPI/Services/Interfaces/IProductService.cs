using BronckhorstAPI.DTO;

namespace BronckhorstAPI.Services.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetAllProducts();
    
    Task<ProductDto> GetProductById(int id);
}