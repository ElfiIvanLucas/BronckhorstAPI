using System.Data;
using System.Data.Common;
using BronckhorstAPI.Application.Filters;
using BronckhorstAPI.Application.Pagination;
using BronckhorstAPI.Infrastructure.Persistence;
using BronckhorstAPI.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BronckhorstAPI.Infrastructure.Tests.Repositories;

public class ProductCatalogSqlTests
{
    [Theory]
    [InlineData(ProductSort.NameAsc)]
    [InlineData(ProductSort.NameDesc)]
    [InlineData(ProductSort.PriceAsc)]
    [InlineData(ProductSort.PriceDesc)]
    public async Task ProductGroups_CanTranslateFilteringGroupingAndPagingToSqlServer(ProductSort sort)
    {
        var commands = new CaptureCommands();
        var options = new DbContextOptionsBuilder<BronckhorstDbContext>()
            .UseSqlServer("Server=unused;Database=preview;Integrated Security=true")
            .AddInterceptors(new SuppressConnection(), commands)
            .Options;
        await using var context = new BronckhorstDbContext(options);

        await new ProductRepository(context).GetProductGroupsAsync(new ProductFilters
        {
            ParentCategoryId = 10, CategoryId = 11, BrandId = 1, Name = "LAL", Sort = sort
        });

        Assert.Equal(2, commands.Sql.Count);
        Assert.Contains("GROUP BY", commands.Sql[1]);
        Assert.Contains("OFFSET", commands.Sql[1]);
    }

    // Keep the real SQL Server query compiler, but never connect or execute a command.
    private sealed class SuppressConnection : DbConnectionInterceptor
    {
        public override ValueTask<InterceptionResult> ConnectionOpeningAsync(DbConnection connection,
            ConnectionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default)
            => ValueTask.FromResult(InterceptionResult.Suppress());
    }

    private sealed class CaptureCommands : DbCommandInterceptor
    {
        public List<string> Sql { get; } = [];

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command,
            CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            Sql.Add(command.CommandText);
            var table = new DataTable();
            if (Sql.Count == 1)
            {
                table.Columns.Add("Count", typeof(int));
                table.Rows.Add(1);
            }
            else
            {
                table.Columns.Add("ProductCode", typeof(string));
                table.Columns.Add("Image", typeof(string));
                table.Columns.Add("ProductCount", typeof(int));
                table.Columns.Add("FromPrice", typeof(decimal));
            }
            return ValueTask.FromResult(InterceptionResult<DbDataReader>.SuppressWithResult(table.CreateDataReader()));
        }
    }
}
