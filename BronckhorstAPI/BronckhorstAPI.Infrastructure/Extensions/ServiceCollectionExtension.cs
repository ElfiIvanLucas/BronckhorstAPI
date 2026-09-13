using System.Diagnostics.CodeAnalysis;
using BronckhorstAPI.Application.Repositories.Interfaces;
using BronckhorstAPI.Domain.Entities;
using BronckhorstAPI.Infrastructure.Persistence;
using BronckhorstAPI.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BronckhorstAPI.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BronckhorstDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("BronckhorstConnection"));
        });

        services.AddTransient<IRepository<Brand>, BrandRepository>();
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<IRepository<Category>, CategoryRepository>();
    }
}
