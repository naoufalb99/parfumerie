using System;
using Burn.Core.Entities;
using Burn.DataAccess.Database;
using Burn.DataAccess.Interfaces;
using Dapper;

namespace Burn.DataAccess.Repositories;

public class ShelfRepository : IShelfRepository
{
    private readonly IPostgreSqlDriver _postgreSqlDriver;

    public ShelfRepository(IPostgreSqlDriver postgreSqlDriver)
    {
        _postgreSqlDriver = postgreSqlDriver;
    }

    public async Task<IList<Shelf>> GetAll()
    {
        var shelfDic = new Dictionary<Guid, Shelf>();

        return (await _postgreSqlDriver.QueryAsync<Shelf, Brand, Perfume, Shelf>(
            @$"SELECT * FROM {TableConstants.ShelfsTableName} s
                        LEFT JOIN {TableConstants.BrandsTableName} b ON s.brand_id = b.id
                        LEFT JOIN {TableConstants.PerfumeShelfTableName} ps ON s.id = ps.shelf_id
                        LEFT JOIN {TableConstants.PerfumesTableName} p ON ps.perfume_id = p.id",
            (shelf, brand, perfume) =>
            {
                if (!shelfDic.TryGetValue(shelf.Id, out var currentShelf))
                {
                    currentShelf = shelf;
                    currentShelf.Brand = brand;
                    currentShelf.Perfumes = new List<Perfume>();
                    shelfDic.Add(currentShelf.Id, currentShelf);
                }
                currentShelf.Perfumes.Add(perfume);
                return currentShelf;
            }, splitOn: "brand_id,perfume_id")).Distinct().ToList();
    }

    public async Task<Shelf> GetById(Guid id)
    {
        var shelfDic = new Dictionary<Guid, Shelf>();

        return (await _postgreSqlDriver.QueryAsync<Shelf, Brand, Perfume, Shelf>(
            @$"SELECT * FROM {TableConstants.ShelfsTableName} s
                        LEFT JOIN {TableConstants.BrandsTableName} b ON s.brand_id = b.id
                        LEFT JOIN {TableConstants.PerfumeShelfTableName} ps ON s.id = ps.shelf_id
                        LEFT JOIN {TableConstants.PerfumesTableName} p ON ps.perfume_id = p.id
                        WHERE s.id = @Id",
            (shelf, brand, perfume) =>
            {
                if(!shelfDic.TryGetValue(shelf.Id, out var currentShelf))
                {
                    currentShelf = shelf;
                    currentShelf.Brand = brand;
                    currentShelf.Perfumes = new List<Perfume>();
                    shelfDic.Add(currentShelf.Id, currentShelf);
                }
                currentShelf.Perfumes.Add(perfume);
                return currentShelf;
            }, new { id }, splitOn: "brand_id,perfume_id")).Distinct().First();
    }

    public async Task Add(Shelf shelf)
    {
        await _postgreSqlDriver.ExecuteAsync($"INSERT INTO {TableConstants.ShelfsTableName} (id, brand_id) VALUES (@Id, @BrandId)", new
        {
            Id = shelf.Id,
            BrandId = shelf?.Brand?.Id
        });
    }

    public async Task Update(Shelf shelf)
    {
        await _postgreSqlDriver.ExecuteAsync($"UPDATE {TableConstants.ShelfsTableName} SET brand_id = @BrandId WHERE id = @Id", new
        {
            Id = shelf.Id,
            BrandId = shelf?.Brand?.Id
        });
    }

    public async Task Delete(Guid id)
    {
        await _postgreSqlDriver.ExecuteAsync(
            $@"DELETE FROM perfume_shelf WHERE shelf_id = @Id;
                DELETE FROM {TableConstants.ShelfsTableName} WHERE id = @Id", new
        {
            Id = id
        });
    }

    public async Task AddPerfumeToShelf(Shelf shelf, Perfume perfume)
    {
        await _postgreSqlDriver.ExecuteAsync(
            $@"INSERT INTO {TableConstants.PerfumeShelfTableName} (perfume_id, shelf_id) VALUES (@PerfumeId, @ShelfId)", new
        {
            PerfumeId = perfume.Id,
            ShelfId = shelf.Id
        });
    }

    public async Task RemovePerfumeFromShelf(Shelf shelf, Perfume perfume)
    {
        await _postgreSqlDriver.ExecuteAsync(
            $@"DELETE FROM {TableConstants.PerfumeShelfTableName} WHERE perfume_id = @PerfumeId AND shelf_id = @ShelfId", new
        {
            PerfumeId = perfume.Id,
            ShelfId = shelf.Id
        });
    }
}

