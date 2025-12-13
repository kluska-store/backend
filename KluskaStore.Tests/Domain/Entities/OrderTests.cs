using FluentAssertions;
using KluskaStore.Domain.Entities;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Tests.Domain.Entities;

using OrderStatus = Order.OrderStatusEnum;

public class OrderTests
{
    private readonly Order _sut = new Order(
        Guid.NewGuid(),
        DateTime.UtcNow.AddDays(-30),
        [
            new OrderItem(Guid.NewGuid(), "product 1", 10, null, 2, new Dictionary<string, string>()),
            new OrderItem(Guid.NewGuid(), "product 2", 20, "Product number 2", 1, new Dictionary<string, string>())
        ]
    );

    public static TheoryData<OrderStatus> OrderStatuses => new(Enum.GetValues<OrderStatus>());

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsValid_ThenCreatesOrder()
    {
        var result = Order.Create(_sut.UserId, _sut.Date, _sut.Items);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeAssignableTo<Order>();

        var order = result.Value;
        order.UserId.Should().Be(_sut.UserId);
        order.Date.Should().Be(_sut.Date);
        order.Status.Should().Be(OrderStatus.OrderReceived);
        order.Items.Should().BeEquivalentTo(_sut.Items);
        order.WasCanceled.Should().BeFalse();
        order.WasDelivered.Should().BeFalse();
        order.WasReturned.Should().BeFalse();
        order.IsActive.Should().BeTrue();
        order.TotalPrice.Should().Be(_sut.Items.Select(i => i.TotalPrice).Sum());
    }

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsInvalid_ThenReturnsFailure()
    {
        var result = Order.Create(Guid.Empty, DateTime.UtcNow.AddDays(1), []);

        result.IsFailure.Should().BeTrue();
        result.Errors.Count.Should().Be(3);
        result.Value.Should().BeNull();
    }

    [Fact]
    public void GivenStatusChange_WhenNewStatusIsCanceled_ThenMarksAsCanceled()
    {
        _sut.Status.Should().Be(OrderStatus.OrderReceived);
        _sut.MarkAs(OrderStatus.Canceled);

        _sut.Status.Should().Be(OrderStatus.Canceled);
        _sut.WasCanceled.Should().BeTrue();
        _sut.WasDelivered.Should().BeFalse();
        _sut.WasReturned.Should().BeFalse();
        _sut.IsActive.Should().BeFalse();
    }

    [Fact]
    public void GivenStatusChange_WhenNewStatusIsDelivered_ThenMarksAsDelivered()
    {
        _sut.Status.Should().Be(OrderStatus.OrderReceived);
        _sut.MarkAs(OrderStatus.Delivered);

        _sut.Status.Should().Be(OrderStatus.Delivered);
        _sut.WasDelivered.Should().BeTrue();
        _sut.WasCanceled.Should().BeFalse();
        _sut.WasReturned.Should().BeFalse();
        _sut.IsActive.Should().BeFalse();
    }

    [Fact]
    public void GivenStatusChange_WhenNewStatusIsReturned_ThenMarksAsDelivered()
    {
        _sut.Status.Should().Be(OrderStatus.OrderReceived);
        _sut.MarkAs(OrderStatus.Returned);

        _sut.Status.Should().Be(OrderStatus.Returned);
        _sut.WasReturned.Should().BeTrue();
        _sut.WasCanceled.Should().BeFalse();
        _sut.WasDelivered.Should().BeFalse();
        _sut.IsActive.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(OrderStatuses))]
    public void GivenStatusChange_ThenChangesStatus(OrderStatus status)
    {
        _sut.MarkAs(status);
        _sut.Status.Should().Be(status);
    }
}
