using System;
using Burn.BL.Interfaces;
using Burn.Core.Entities;
using Burn.DataAccess.Interfaces;

namespace Burn.BL.Services;

public class PerfumeService : IPerfumeService
{
    private readonly IPerfumeRepository _perfumeRepository;

    public PerfumeService(IPerfumeRepository perfumeRepository)
    {
        _perfumeRepository = perfumeRepository;
    }

    public Task AddPerfume(Perfume perfume)
        => _perfumeRepository.Add(perfume);

    public async Task AddPromoToPerfume(Perfume perfume, decimal discount)
    {
        if(discount < 0)
        {
            throw new ArgumentException("The discount given can't be negative");
        }
        if(perfume.Price - discount < 0)
        {
            throw new ArgumentException("The discount given is higher than the price");
        }
        perfume.Price -= discount;
        perfume.Discount += discount;
        await _perfumeRepository.Update(perfume);
    }

    public Task DeletePerfume(Guid id)
        => _perfumeRepository.Delete(id);

    public Task<IList<Perfume>> GetAllPerfumes()
        => _perfumeRepository.GetAll();

    public Task<Perfume> GetPerfumeById(Guid id)
        => _perfumeRepository.GetById(id);

    public bool IsOutOfStock(Perfume perfume)
        => perfume.Quantity <= 0;

    public Task UpdatePerfume(Perfume perfume)
        => _perfumeRepository.Update(perfume);
}

