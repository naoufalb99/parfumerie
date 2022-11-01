using System;
using Burn.Core.Entities;

namespace Burn.BL.Interfaces;

public interface IPerfumeService
{
    Task<Perfume> GetPerfumeById(Guid id);
    Task<IList<Perfume>> GetAllPerfumes();
    Task AddPerfume(Perfume perfume);
    Task UpdatePerfume(Perfume perfume);
    Task DeletePerfume(Guid id);
    bool IsOutOfStock(Perfume perfume);
    Task AddPromoToPerfume(Perfume perfume, decimal discount);
}

