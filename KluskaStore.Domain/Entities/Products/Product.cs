using KluskaStore.Domain.Entities.Generics;
using KluskaStore.Domain.Errors.Entities;

namespace KluskaStore.Domain.Entities.Products;

public class Product : DefaultIdentityEntity
{
    private readonly Dictionary<string, string> _specifications;

    private Product()
    {
        _specifications = null!;
        Name = null!;
        IsAvailable = true;
    }

    internal Product(decimal price, string name)
    {
        _specifications = new Dictionary<string, string>();
        Price = price;
        Name = name;
        IsAvailable = true;
    }

    public IReadOnlyDictionary<string, string> Specifications => _specifications.AsReadOnly();
    public decimal Price { get; protected set; }
    public string Name { get; protected set; }
    public string? Description { get; protected set; }
    public bool IsAvailable { get; protected set; }

    public static Result<Product> Create(
        decimal price,
        string name
    )
    {
        Error? error = null;
        if (price <= 0) error = ProductErrors.InvalidPrice;
        else if (string.IsNullOrWhiteSpace(name)) error = ProductErrors.EmptyName;

        return error is null
            ? Result<Product>.Success(new Product(price, name))
            : Result<Product>.Failure(error);
    }

    public void PatchSpecifications(IEnumerable<KeyValuePair<string, string?>> patch)
    {
        foreach (var (key, val) in patch)
        {
            if (string.IsNullOrWhiteSpace(val)) _specifications.Remove(key);
            else _specifications[key] = val;
        }
    }

    public Result<Product> ChangePrice(decimal newPrice)
    {
        if (newPrice <= 0) return Result<Product>.Failure(ProductErrors.InvalidPrice);

        Price = newPrice;
        return Result<Product>.Success(this);
    }

    public Result<Product> ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName)) return Result<Product>.Failure(ProductErrors.EmptyName);

        Name = newName;
        return Result<Product>.Success(this);
    }

    public void ChangeDescription(string? newDescription) => Description = newDescription;

    public void MarkAsUnavailable() => IsAvailable = false;
    public void MarkAsAvailable() => IsAvailable = true;
}
