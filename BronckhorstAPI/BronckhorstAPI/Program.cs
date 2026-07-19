using BronckhorstAPI.DTO;
using BronckhorstAPI.Mappers;
using BronckhorstAPI.Mappers.Interfaces;
using BronckhorstAPI.Services;
using BronckhorstAPI.Services.Interfaces;
using Domain.Entities;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;
using Persistence.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BronckhorstDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BronckhorstConnection"));
});

builder.Services.AddTransient<IRepository<Product>, ProductRepository>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IMapper<Product, ProductDto>, ProductMapper>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
