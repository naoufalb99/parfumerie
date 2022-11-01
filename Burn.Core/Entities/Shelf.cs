namespace Burn.Core.Entities;

public class Shelf
{
    public Guid Id { get; set; }
    public Brand Brand { get; set; }
    public IList<Perfume> Perfumes { get; set; } = new List<Perfume>();
}

