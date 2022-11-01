using System.Data;
using Npgsql;
using Dapper;
using System.Linq;
using Burn.Core.Entities;
using Burn.DataAccess.Interfaces;
using Burn.DataAccess.Database;

namespace Burn.DataAccess.Repositories;

public class PerfumeRepository : IPerfumeRepository
{
    private readonly IPostgreSqlDriver _postgreSqlDriver;

    public PerfumeRepository(IPostgreSqlDriver postgreSqlDriver)
    {
        _postgreSqlDriver = postgreSqlDriver;
    }

    public async Task<IList<Perfume>> GetAll()
    {

        return (await _postgreSqlDriver.QueryAsync<Perfume, Brand, Perfume>(
            $"SELECT * FROM {TableConstants.PerfumesTableName} p LEFT JOIN brands b ON p.brand_id = b.id",
            (perfume, brand) =>
            {
                perfume.Brand = brand;
                return perfume;
            })
            ).ToList();
    }

    public async Task<Perfume> GetById(Guid id)
    {
        return (await _postgreSqlDriver.QueryAsync<Perfume, Brand, Perfume>(
            @$"SELECT * FROM {TableConstants.PerfumesTableName} p LEFT JOIN brands b ON p.brand_id = b.id WHERE p.id = @Id",
            (perfume, brand) => {
                perfume.Brand = brand;
                return perfume;
            }, new { id })).Single();
    }

    public async Task Add(Perfume perfume)
    {
        await _postgreSqlDriver.ExecuteAsync($"INSERT INTO {TableConstants.PerfumesTableName} (id, name, price, quantity, brand_id) VALUES (@Id, @Name, @Price, @Quantity, @BrandId)", new
        {
            Id = perfume.Id,
            Name = perfume.Name,
            Price = perfume.Price,
            Quantity = perfume.Quantity,
            BrandId = perfume.Brand.Id
        });
    }

    public async Task Update(Perfume perfume)
    {
        await _postgreSqlDriver.ExecuteAsync(
            $@"UPDATE {TableConstants.PerfumesTableName} SET name = @Name, price = @Price, quantity = @Quantity, discount = @Discount, brand_id = @BrandId WHERE id = @Id", new
        {
            Id = perfume.Id,
            Name = perfume.Name,
            Price = perfume.Price,
            Quantity = perfume.Quantity,
            Discount = perfume.Discount,
            BrandId = perfume.Brand.Id
        });
    }

    public async Task Delete(Guid id)
    {
        await _postgreSqlDriver.ExecuteAsync(
            $@"DELETE FROM {TableConstants.PerfumeShelfTableName} WHERE perfume_id = @Id;
            DELETE FROM {TableConstants.PerfumesTableName} WHERE id = @Id",
            new { id });
    }
}
