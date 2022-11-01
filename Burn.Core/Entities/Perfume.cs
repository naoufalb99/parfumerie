namespace Burn.Core.Entities;

public class Perfume
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Discount { get; set; }
    public Brand Brand { get; set; }
}

