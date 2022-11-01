using Burn.BL.Interfaces;
using Burn.BL.Services;
using Burn.Core.Entities;
using Burn.DataAccess;
using Burn.DataAccess.Interfaces;
using Burn.DataAccess.Repositories;

namespace Burn.Tests.BL.Services;

public class PerfumeServiceTest
{
    private readonly IPerfumeService _perfumeService;
    private readonly IPerfumeRepository _perfumeRepository;
    private readonly Fixture _fixture;

    public PerfumeServiceTest()
    {
        _perfumeRepository = Substitute.For<IPerfumeRepository>();
        _perfumeService = new PerfumeService(_perfumeRepository);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task ShouldReturnPerfumes()
    {
        // Arrange
        var perfumes = _fixture.CreateMany<Perfume>().ToList();
        _perfumeRepository.GetAll().Returns(perfumes);

        // Act
        var result = await _perfumeService.GetAllPerfumes();

        // Assert
        Assert.Equal(result, perfumes);
    }

    [Fact]
    public async Task ShouldReturnPerfume()
    {
        // Arrange
        var perfume = _fixture.Create<Perfume>();
        _perfumeRepository.GetById(perfume.Id).Returns(perfume);

        // Act
        var result = await _perfumeService.GetPerfumeById(perfume.Id);

        // Assert
        Assert.Equal(result, perfume);
    }

    [Fact]
    public async Task ShouldDeletePerfume()
    {
        // Arrange
        var perfume = _fixture.Create<Perfume>();

        // Act
        await _perfumeService.DeletePerfume(perfume.Id);

        // Assert
        await _perfumeRepository.Received().Delete(perfume.Id);
    }

    [Fact]
    public async Task ShouldUpdatePerfume()
    {
        // Arrange
        var perfume = _fixture.Create<Perfume>();

        // Act
        await _perfumeService.UpdatePerfume(perfume);

        // Assert
        await _perfumeRepository.Received().Update(perfume);
    }

    [Fact]
    public async Task ShouldAddPerfume()
    {
        // Arrange
        var perfume = _fixture.Create<Perfume>();

        // Act
        await _perfumeService.AddPerfume(perfume);

        // Assert
        await _perfumeRepository.Received().Add(perfume);
    }

    [Fact]
    public void IsOutOfStockShouldReturnTrue()
    {
        // Arrange
        var perfume = _fixture.Build<Perfume>()
            .With(p => p.Quantity, 0)
            .Create();

        // Act
        var result = _perfumeService.IsOutOfStock(perfume);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsOutOfStockShouldReturnFalse()
    {
        // Arrange
        var perfume = _fixture.Build<Perfume>()
            .With(p => p.Quantity, 10)
            .Create();

        // Act
        var result = _perfumeService.IsOutOfStock(perfume);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task AddPromoToPerfumeShouldThrowExceptionWhenDiscountIsLessThanZero()
    {
        // Arrange
        var perfume = _fixture.Create<Perfume>();
        decimal discount = -10;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _perfumeService.AddPromoToPerfume(perfume, discount));
    }

    [Fact]
    public async Task AddPromoToPerfumeShouldThrowExceptionWhenDiscountGreaterThanPrice()
    {
        // Arrange
        var perfume = _fixture.Build<Perfume>()
            .With(p => p.Price, 10)
            .Create();
        decimal discount = 100;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _perfumeService.AddPromoToPerfume(perfume, discount));
    }

    [Fact]
    public async Task AddPromoToPerfumeShouldShouldUpdate()
    {
        // Arrange
        decimal perfumePrice = 10;
        decimal perfumeDiscount = 0;
        decimal discount = 5;
        var perfume = _fixture.Build<Perfume>()
            .With(p => p.Price, perfumePrice)
            .With(p => p.Discount, perfumeDiscount)
            .Create();

        // Act
        await _perfumeService.AddPromoToPerfume(perfume, discount);

        // Assert
        await _perfumeRepository.Received().Update(perfume);
        Assert.Equal(perfume.Price, perfumePrice - discount);
        Assert.Equal(perfume.Discount, perfumeDiscount + discount);
    }
}

