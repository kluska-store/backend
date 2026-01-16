using KluskaStore.Domain.Entities.Payments;
using KluskaStore.Domain.Errors.Entities;
using KluskaStore.Tests.Common.Builders;

namespace KluskaStore.Tests.Domain.Entities.Payments;

public class TransferenceTests
{
    private readonly Transference _sut = TransferenceBuilder.Valid();

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsValid_ThenCreatesTransference()
    {
        var result = Transference.Create(_sut.ReceiverStoreId, _sut.Value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        var transference = result.Value;
        transference.ReceiverStoreId.Should().Be(_sut.ReceiverStoreId);
        transference.Value.Should().Be(_sut.Value);
    }

    [Fact]
    public void GivenEntityCreation_WhenStoreIdIsEmpty_ThenReturnsFailure()
    {
        var result = Transference.Create(Guid.Empty, _sut.Value);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(TransferenceErrors.EmptyReceiverId.Code);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void GivenEntityCreation_WhenTransferedValueIsZeroOrNegative_ThenReturnsFailure(decimal value)
    {
        var result = Transference.Create(_sut.ReceiverStoreId, value);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(TransferenceErrors.InvalidTransference.Code);
    }
}
