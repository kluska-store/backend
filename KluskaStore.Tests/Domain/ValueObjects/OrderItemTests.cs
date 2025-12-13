using FluentAssertions;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Tests.Domain.ValueObjects;

public class OrderItemTests
{
    [Fact]
    public void GivenValueObjectCreation_WhenDataIsValid_ThenCreatesOrderItem()
    {
        var productId = Guid.NewGuid();
        var name = "name";
        var unitPrice = 10m;
        var description = "description";
        var quantity = 2u;
        var specifications = new Dictionary<string, string>() { ["size"] = "10cm x 10cm" };
        var result = OrderItem.Create(productId, name, unitPrice, description, quantity, specifications);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeAssignableTo<OrderItem>();

        var orderItem = result.Value;
        orderItem.ProductId.Should().Be(productId);
        orderItem.Name.Should().Be(name);
        orderItem.UnitPrice.Should().Be(unitPrice);
        orderItem.Description.Should().Be(description);
        orderItem.Quantity.Should().Be(quantity);
        orderItem.Specifications.Should().BeEquivalentTo(specifications);
        orderItem.TotalPrice.Should().Be(unitPrice * quantity);
    }

    [Fact]
    public void GivenValueObjectCreation_WhenDataIsInvalid_ThenReturnsFailure()
    {
        var result = OrderItem.Create(Guid.Empty, null!, -10, null, 0, null!);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Value.Should().BeNull();
    }
}
