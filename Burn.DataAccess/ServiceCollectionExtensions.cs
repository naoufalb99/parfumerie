using System;
using Burn.DataAccess.Database;
using Burn.DataAccess.Interfaces;
using Burn.DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Burn.DataAccess;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UsePostgreSql(this IServiceCollection services, IConfigurationSection configuration)
        => services
            .Configure<PostgreSqlConfiguration>(configuration)
            .AddTransient(x => x.GetService<IOptions<PostgreSqlConfiguration>>().Value)
            .AddTransient<IBuildPostgreSqlConnection, PostgreSqlConnectionFactory>()
            .AddTransient<IPostgreSqlDriver, PostgreSqlDriver>()
            .UsePostgreSqlRepositories();

    private static IServiceCollection UsePostgreSqlRepositories(this IServiceCollection services)
        => services
            .AddTransient<IBrandRepository, BrandRepository>()
            .AddTransient<IPerfumeRepository, PerfumeRepository>()
            .AddTransient<IShelfRepository, ShelfRepository>();

}
