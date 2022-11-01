using System;
using Burn.Core.Entities;

namespace Burn.DataAccess.Interfaces;

public interface IShelfRepository
{
    Task<IList<Shelf>> GetAll();
    Task<Shelf> GetById(Guid id);
    Task Add(Shelf shelf);
    Task Update(Shelf shelf);
    Task Delete(Guid id);
    Task AddPerfumeToShelf(Shelf shelf, Perfume perfume);
    Task RemovePerfumeFromShelf(Shelf shelf, Perfume perfume);
}

