using System.Diagnostics.CodeAnalysis;
using BronckhorstAPI.Application.DTO;
using BronckhorstAPI.Application.Mappers;
using BronckhorstAPI.Application.Mappers.Interfaces;
using BronckhorstAPI.Application.Services;
using BronckhorstAPI.Application.Services.Interfaces;
using BronckhorstAPI.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace BronckhorstAPI.Application.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddServices(services);
        AddMappers(services);
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddTransient<IBrandService, BrandService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<IProductService, ProductService>();
    }

    private static void AddMappers(IServiceCollection services)
    {
        services.AddTransient<IMapper<Brand, BrandDto>, BrandMapper>();
        services.AddTransient<IMapper<Category, CategoryDto>, CategoryMapper>();
        services.AddTransient<IMapper<Product, ProductDto>, ProductMapper>();
    }
}
