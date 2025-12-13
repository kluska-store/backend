using KluskaStore.Domain.Entities.Payments;

namespace KluskaStore.Tests.Domain.Entities.Payments;

public class TransferenceTests
{
    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsValid_ThenCreatesTransference()
    {
        var storeId = Guid.NewGuid();
        var value = 20m;
        var result = Transference.Create(storeId, value);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeAssignableTo<Transference>();

        var transference = result.Value;
        transference.ReceiverStoreId.Should().Be(storeId);
        transference.Value.Should().Be(value);
    }

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsInvalid_ThenReturnsFailure()
    {
        var result = Transference.Create(Guid.Empty, -1);

        result.IsFailure.Should().BeTrue();
        result.Errors.Count.Should().Be(2);
        result.Value.Should().BeNull();
    }
}
