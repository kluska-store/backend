using FluentAssertions;
using KluskaStore.Domain.Entities.Product;

namespace KluskaStore.Tests.Domain.Entities.Product;

public class ItemTests
{
    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsValid_ThenCreatesItem()
    {
        var product = new KluskaStore.Domain.Entities.Product.Product(new Dictionary<string, string>(), 10, "product");
        uint quantity = 10;
        var result = Item.Create(product, quantity);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeAssignableTo<Item>();
        result.Value.Product.Should().Be(product);
        result.Value.Quantity.Should().Be(quantity);
    }

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsInvalid_ThenReturnsFailure()
    {
        var result = Item.Create(null!, 0);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Value.Should().BeNull();
    }
}
