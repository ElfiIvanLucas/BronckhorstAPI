using BronckhorstAPI.Application.DTO;

namespace BronckhorstAPI.Application.Services.Interfaces;

public interface IBrandService
{
    Task<List<BrandDto>> GetAllBrandsAsync();
}
