using KluskaStore.Domain.Entities.Payments;

namespace KluskaStore.Tests.Domain.Entities.Payments;

public class PaymentTests
{
    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsValid_ThenCreatesPayment()
    {
        var userId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var date = DateTime.UtcNow.AddDays(-30);
        List<Transference> transferences = [new(Guid.NewGuid(), 10)];
        var result = Payment.Create(userId, date, orderId, transferences);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeAssignableTo<Payment>();

        var payment = result.Value;
        payment.PayerUserId.Should().Be(userId);
        payment.OrderId.Should().Be(orderId);
        payment.Date.Should().Be(date);
        payment.Transferences.Should().BeEquivalentTo(transferences);
        payment.Value.Should().Be(transferences.Select(t => t.Value).Sum());
    }

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsInvalid_ThenReturnsFailure()
    {
        var result = Payment.Create(Guid.Empty, DateTime.UtcNow.AddDays(1), Guid.Empty, new List<Transference>());

        result.IsFailure.Should().BeTrue();
        result.Errors.Count.Should().Be(4);
        result.Value.Should().BeNull();
    }
}
