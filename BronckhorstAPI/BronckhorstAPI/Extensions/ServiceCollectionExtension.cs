using BronckhorstAPI.DTO;
using BronckhorstAPI.Mappers;
using BronckhorstAPI.Mappers.Interfaces;
using BronckhorstAPI.Services;
using BronckhorstAPI.Services.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;
using Persistence.Repositories.Interfaces;

namespace BronckhorstAPI.Extensions;

public static class ServiceCollectionExtension
{
    public static void ConfigureServices(this IServiceCollection services,  IConfiguration configuration)
    {
        services.AddDbContext<BronckhorstDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("BronckhorstConnection"));
        });
        
        
        services.AddTransient<IRepository<Product>, ProductRepository>();
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<IMapper<Product, ProductDto>, ProductMapper>();
    }
}