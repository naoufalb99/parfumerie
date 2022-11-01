using System;
using Burn.Core.Entities;

namespace Burn.DataAccess.Interfaces;

public interface IBrandRepository
{
    Task<IList<Brand>> GetAll();
    Task<Brand> GetById(Guid id);
    Task Add(Brand brand);
    Task Update(Brand brand);
    Task Delete(Guid id);
}

