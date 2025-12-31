using KluskaStore.Domain.Errors.ValueObjects;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Tests.Domain.ValueObjects;

public class OrderItemTests
{
    private readonly OrderItem _sut = new(
        Guid.NewGuid(),
        "name",
        10,
        "description",
        2,
        new Dictionary<string, string>
        {
            ["material"] = "metal",
            ["category"] = "RPG Games"
        }
    );

    [Fact]
    public void GivenValueObjectCreation_WhenDataIsValid_ThenCreatesOrderItem()
    {
        var result = OrderItem.Create(
            _sut.ProductId,
            _sut.Name,
            _sut.UnitPrice,
            _sut.Description,
            _sut.Quantity,
            _sut.Specifications
        );

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        var orderItem = result.Value;
        orderItem.ProductId.Should().Be(_sut.ProductId);
        orderItem.Name.Should().Be(_sut.Name);
        orderItem.UnitPrice.Should().Be(_sut.UnitPrice);
        orderItem.Description.Should().Be(_sut.Description);
        orderItem.Quantity.Should().Be(_sut.Quantity);
        orderItem.Specifications.Should().BeEquivalentTo(_sut.Specifications);
    }

    [Fact]
    public void GivenEntityCreation_WhenProductIdIsEmpty_ThenReturnsFailure()
    {
        var result = OrderItem.Create(
            Guid.Empty,
            _sut.Name,
            _sut.UnitPrice,
            _sut.Description,
            _sut.Quantity,
            _sut.Specifications
        );

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(OrderItemErrors.EmptyProductId.Code);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void GivenEntityCreation_WhenUnitPriceIsZeroOrNegative_ThenReturnsFailure(decimal unitPrice)
    {
        var result = OrderItem.Create(
            _sut.ProductId,
            _sut.Name,
            unitPrice,
            _sut.Description,
            _sut.Quantity,
            _sut.Specifications
        );

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(OrderItemErrors.InvalidUnitPrice.Code);
    }

    [Fact]
    public void GivenEntityCreation_WhenNameIsEmpty_ThenReturnsFailure()
    {
        var result = OrderItem.Create(
            _sut.ProductId,
            "",
            _sut.UnitPrice,
            _sut.Description,
            _sut.Quantity,
            _sut.Specifications
        );

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(OrderItemErrors.EmptyName.Code);
    }

    [Fact]
    public void GivenEntityCreation_WhenQuantityIsZero_ThenReturnsFailure()
    {
        var result = OrderItem.Create(
            _sut.ProductId,
            _sut.Name,
            _sut.UnitPrice,
            _sut.Description,
            0,
            _sut.Specifications
        );

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(OrderItemErrors.InvalidQuantity.Code);
    }

    [Fact]
    public void GivenTotalPriceCalculation_ThenReturnsQuantityTimesUnitPrice() =>
        _sut.TotalPrice.Should().Be(_sut.Quantity * _sut.UnitPrice);
}
