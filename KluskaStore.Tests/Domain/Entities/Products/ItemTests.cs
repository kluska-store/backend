using KluskaStore.Domain.Entities.Products;
using KluskaStore.Domain.Errors.Entities;

namespace KluskaStore.Tests.Domain.Entities.Products;

public class ItemTests
{
    private readonly Item _sut = new(new Product(10, "product"), 10);

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsValid_ThenCreatesItem()
    {
        var result = Item.Create(_sut.Product, _sut.Quantity);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Product.Should().Be(_sut.Product);
        result.Value.Quantity.Should().Be(_sut.Quantity);
    }

    [Fact]
    public void GivenEntityCreation_WhenQuantityIsZero_ThenReturnsFailure()
    {
        var result = Item.Create(_sut.Product, 0);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(ItemErrors.EmptyItem.Code);
    }
}
