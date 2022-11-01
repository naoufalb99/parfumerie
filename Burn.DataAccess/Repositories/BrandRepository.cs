using System.Data;
using Npgsql;
using Dapper;
using System.Linq;
using Burn.Core.Entities;
using Burn.DataAccess.Interfaces;
using Burn.DataAccess.Database;

namespace Burn.DataAccess.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly IPostgreSqlDriver _postgreSqlDriver;

    public BrandRepository(IPostgreSqlDriver postgreSqlDriver)
    {
        _postgreSqlDriver = postgreSqlDriver;
    }

    public async Task<IList<Brand>> GetAll()
    {
        return (await _postgreSqlDriver.QueryAsync<Brand>($"SELECT * FROM {TableConstants.BrandsTableName}")).ToList();
    }

    public async Task<Brand> GetById(Guid id)
    {
        return await _postgreSqlDriver.QuerySingleAsync<Brand>($"SELECT * FROM {TableConstants.BrandsTableName} WHERE id = @Id", new { id });
    }

    public async Task Add(Brand brand)
    {
        await _postgreSqlDriver.ExecuteAsync($"INSERT INTO {TableConstants.BrandsTableName} (id, name) VALUES (@Id, @Name)", new
        {
            Id = brand.Id,
            Name = brand.Name
        });
    }

    public async Task Update(Brand brand)
    {
        await _postgreSqlDriver.ExecuteAsync($"UPDATE {TableConstants.BrandsTableName} SET name = @Name WHERE id = @Id", new
        {
            Id = brand.Id,
            Name = brand.Name
        });
    }

    public async Task Delete(Guid id)
    {
        await _postgreSqlDriver.ExecuteAsync(
            $@"UPDATE {TableConstants.PerfumesTableName} SET brand_id = NULL WHERE brand_id = @Id;
                DELETE FROM {TableConstants.BrandsTableName} WHERE id = @Id", new { id });
    }
}