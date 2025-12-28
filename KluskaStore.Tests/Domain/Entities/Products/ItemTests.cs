using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Tests.Domain.Entities.Products;

public class ItemTests
{
    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsValid_ThenCreatesItem()
    {
        var product = new Product(new Dictionary<string, string>(), 10, "product");
        uint quantity = 10;
        var result = Item.Create(product, quantity);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Product.Should().Be(product);
        result.Value.Quantity.Should().Be(quantity);
    }

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsInvalid_ThenReturnsFailure()
    {
        var result = Item.Create(null!, 0);

        result.IsFailure.Should().BeTrue();
    }
}
