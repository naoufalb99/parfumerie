using Burn.BL.Interfaces;
using Burn.BL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Burn.BL;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseBLServices(this IServiceCollection services)
        => services
            .AddTransient<IPerfumeService, PerfumeService>();

}

