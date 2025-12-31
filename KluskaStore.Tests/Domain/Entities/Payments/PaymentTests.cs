using KluskaStore.Domain.Entities.Payments;
using KluskaStore.Domain.Errors.Entities;

namespace KluskaStore.Tests.Domain.Entities.Payments;

public class PaymentTests
{
    private readonly Payment _sut = new(
        Guid.NewGuid(),
        DateTime.UtcNow.AddDays(-30),
        Guid.NewGuid(),
        [new Transference(Guid.NewGuid(), 10)]
    );

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsValid_ThenCreatesPayment()
    {
        var result = Payment.Create(_sut.PayerUserId, _sut.Date, _sut.OrderId, _sut.Transferences);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        var payment = result.Value;
        payment.PayerUserId.Should().Be(_sut.PayerUserId);
        payment.Date.Should().Be(_sut.Date);
        payment.OrderId.Should().Be(_sut.OrderId);
        payment.Transferences.Should().BeEquivalentTo(_sut.Transferences);
    }

    [Fact]
    public void GivenEntityCreation_WhenUserIdIsEmpty_ThenReturnsFailure()
    {
        var result = Payment.Create(Guid.Empty, _sut.Date, _sut.OrderId, _sut.Transferences);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(PaymentErrors.EmptyUserId.Code);
    }

    [Fact]
    public void GivenEntityCreation_WhenPaymentDateIsInTheFuture_ThenReturnsFailure()
    {
        var result = Payment.Create(_sut.PayerUserId, DateTime.UtcNow.AddDays(1), _sut.OrderId, _sut.Transferences);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(PaymentErrors.InvalidPaymentDate.Code);
    }

    [Fact]
    public void GivenEntityCreation_WhenOrderIdIsEmpty_ThenReturnsFailure()
    {
        var result = Payment.Create(_sut.PayerUserId, _sut.Date, Guid.Empty, _sut.Transferences);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(PaymentErrors.EmptyOrderId.Code);
    }

    [Fact]
    public void GivenEntityCreation_WhenNoTransferencesAreRegistered_ThenReturnsFailure()
    {
        var result = Payment.Create(_sut.PayerUserId, _sut.Date, _sut.OrderId, []);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(PaymentErrors.EmptyPayment.Code);
    }

    [Fact]
    public void GivenTotalValueCalculation_ThenReturnsTheSumOfTheValueOfEachTransference()
    {
        var expectedValue = _sut.Transferences.Select(t => t.Value).Sum();
        _sut.Value.Should().Be(expectedValue);
    }
}
