using Burn.DataAccess;
using Microsoft.Extensions.Configuration;
using Burn.Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using Burn.DataAccess.Interfaces;
using Newtonsoft.Json;
using Burn.BL;

namespace Burn.Runner;

public class Program
{
    private static IConfigurationRoot? config;

    static async Task Main(string[] args)
    {
        Initialize();

        // Console.WriteLine(config.GetDebugView());

        var serviceProvider = new ServiceCollection()
            .UsePostgreSql(config?.GetSection("PostgreSql"))
            .UseBLServices()
            .BuildServiceProvider();

        var perfumeRepo = serviceProvider.GetService<IPerfumeRepository>();
        var brandRepo = serviceProvider.GetService<IBrandRepository>();
        var shelfRepo = serviceProvider.GetService<IShelfRepository>();

        var allPerfumes = await perfumeRepo.GetAll();
        var allBrands = await brandRepo.GetAll();
        var allShelfs = await shelfRepo.GetAll();

        Console.WriteLine($"perfumeTotal = {allPerfumes.Count}");
        Console.WriteLine($"brandTotal = {allBrands.Count}");
        Console.WriteLine($"shelfTotal = {allShelfs.Count}");

        string allShelfsJSON = JsonConvert.SerializeObject(allShelfs, Formatting.Indented);
        Console.WriteLine(allShelfsJSON);

        if (allPerfumes.Count == 0 && allBrands.Count == 0 && allShelfs.Count == 0)
        {
            var newBrand = new Brand()
            {
                Id = Guid.NewGuid(),
                Name = "Dior"
            };

            await brandRepo.Add(newBrand);

            var newShelf = new Shelf()
            {
                Id = Guid.NewGuid(),
                Brand = newBrand
            };

            await shelfRepo.Add(newShelf);

            var newPerfume1 = new Perfume()
            {
                Id = Guid.NewGuid(),
                Name = "Perfume test 1",
                Price = 110,
                Quantity = 11,
                Brand = newBrand
            };

            var newPerfume2 = new Perfume()
            {
                Id = Guid.NewGuid(),
                Name = "Perfume test 2",
                Price = 220,
                Quantity = 22,
                Brand = newBrand
            };

            var newPerfume3 = new Perfume()
            {
                Id = Guid.NewGuid(),
                Name = "Perfume test 3",
                Price = 330,
                Quantity = 33,
                Brand = newBrand
            };

            await perfumeRepo.Add(newPerfume1);
            await perfumeRepo.Add(newPerfume2);
            await perfumeRepo.Add(newPerfume3);

            await shelfRepo.AddPerfumeToShelf(newShelf, newPerfume1);
            await shelfRepo.AddPerfumeToShelf(newShelf, newPerfume2);
            await shelfRepo.AddPerfumeToShelf(newShelf, newPerfume3);
        }

        
    }

    private static void Initialize()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        config = builder.Build();
    }
}

