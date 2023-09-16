using ElecShop.WebApi.DataLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElecShop.WebApi.Core.Utilities.Extensions.Connection
{
    public static class ConnectionExtension
    {
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection service,IConfiguration configuration)
        {
            service.AddDbContextPool<ElecShopDbContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ShoppingConnectionString"),
                        x => x.MigrationsHistoryTable("__EFMigrationsHistory", "dbo"));

                options.EnableSensitiveDataLogging();
            });

            return service;
        }
    }
}
