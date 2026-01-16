using KluskaStore.Domain.Entities.Products;
using KluskaStore.Domain.Errors.Entities;
using KluskaStore.Tests.Common.Builders;

namespace KluskaStore.Tests.Domain.Entities.Products;

public class CartTests : ProductCollectionTests
{
    private readonly Cart _sut = CartBuilder.Valid();
    protected override ProductCollection CreateSut() => new Cart(_sut.UserId);

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsValid_ThenCreatesCart()
    {
        var result = Cart.Create(_sut.UserId);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.UserId.Should().Be(_sut.UserId);
        result.Value.Items.Should().BeEquivalentTo(_sut.Items.Where(i => i.Quantity > 0));
    }

    [Fact]
    public void GivenEntityCreation_WhenUserIsIsEmpty_ThenReturnsFailure()
    {
        var result = Cart.Create(Guid.Empty);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(CartErrors.EmptyUserId.Code);
    }
}
