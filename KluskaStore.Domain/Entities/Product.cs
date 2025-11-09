namespace KluskaStore.Domain.Entities;

public class Product : Entity<uint> {
  // Only sample code
  public string Name { get; set; }

  public Product() { }

  public Product(uint id, string name) : base(id) {
    Name = name;
  }
}