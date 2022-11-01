using System;
using Burn.Core.Entities;

namespace Burn.DataAccess.Interfaces;

public interface IPerfumeRepository
{
    Task<IList<Perfume>> GetAll();
    Task<Perfume> GetById(Guid id);
    Task Add(Perfume perfume);
    Task Update(Perfume perfume);
    Task Delete(Guid id);
}
