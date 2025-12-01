using FluentAssertions;
using KluskaStore.Domain.Entities;

namespace KluskaStore.Tests.Domain.Entities;

public class ItemTests
{
    private readonly Item _sut = new(
        new Product(
            new Dictionary<string, string>(),
            10,
            "product"
        ),
        2
    );

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsValid_ThenCreatesItem()
    {
        var result = Item.Create(_sut.Product, _sut.Quantity);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeAssignableTo<Item>();
        result.Value.Product.Should().Be(_sut.Product);
        result.Value.Quantity.Should().Be(_sut.Quantity);
    }

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsInvalid_ThenReturnsFailure()
    {
        var result = Item.Create(null!, 0);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Value.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(20)]
    [InlineData(1)]
    public void GivenQuantityChange_ThenChangesQuantityAndReturnsItemValidity(uint amount)
    {
        _sut.ChangeQuantity(amount).Should().Be(_sut.Quantity != 0);
        _sut.Quantity.Should().Be(amount);
    }
}
