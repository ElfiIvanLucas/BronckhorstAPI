using BronckhorstAPI.Application.DTO;
using BronckhorstAPI.Application.Mappers.Interfaces;
using BronckhorstAPI.Application.Repositories.Interfaces;
using BronckhorstAPI.Application.Services.Interfaces;
using BronckhorstAPI.Domain.Entities;

namespace BronckhorstAPI.Application.Services;

public class BrandService : IBrandService
{
    private readonly IMapper<Brand, BrandDto> _brandMapper;
    private readonly IRepository<Brand> _brandRepository;

    public BrandService(IRepository<Brand> brandRepository, IMapper<Brand, BrandDto> brandMapper)
    {
        _brandRepository = brandRepository ?? throw new ArgumentNullException(nameof(brandRepository));
        _brandMapper = brandMapper ?? throw new ArgumentNullException(nameof(brandMapper));
    }

    public async Task<List<BrandDto>> GetAllBrandsAsync()
    {
        var brandEntities = await _brandRepository.GetAllAsync();

        return [.. brandEntities.Select(_brandMapper.MapToDto)];
    }
}
