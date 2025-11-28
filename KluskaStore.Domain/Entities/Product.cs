using KluskaStore.Domain.Shared;

namespace KluskaStore.Domain.Entities;

public class Product : Entity<uint>
{
    private readonly Dictionary<string, string> _specifications;

    public IReadOnlyDictionary<string, string> Specifications => _specifications.AsReadOnly();
    public decimal Price { get; protected set; }
    public string Name { get; protected set; }
    public string? Description { get; protected set; }

    private Product() { }

    internal Product(Dictionary<string, string> specifications, decimal price, string name)
    {
        _specifications = specifications;
        Price = price;
        Name = name;
    }

    public static Result<Product> Create(
        IEnumerable<KeyValuePair<string, string>> specifications,
        decimal price,
        string name
    )
    {
        List<string> errors = [];

        if (price <= 0) errors.Add("Price must be grater than 0");
        if (string.IsNullOrWhiteSpace(name)) errors.Add("Name must not be empty");

        return errors.Count > 0
            ? Result<Product>.Failure(errors)
            : Result<Product>.Success(new Product(specifications.ToDictionary(), price, name));
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
        if (newPrice <= 0) return Result<Product>.Failure("Price must be grater than 0");

        Price = newPrice;
        return Result<Product>.Success(this);
    }

    public Result<Product> ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName)) return Result<Product>.Failure("Name must not be empty");

        Name = newName;
        return Result<Product>.Success(this);
    }

    public void ChangeDescription(string? newDescription) => Description = newDescription;
}
