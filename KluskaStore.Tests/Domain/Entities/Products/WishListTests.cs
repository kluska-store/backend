using KluskaStore.Domain.Entities.Products;
using KluskaStore.Domain.Errors.Entities;
using KluskaStore.Tests.Common.Builders;

namespace KluskaStore.Tests.Domain.Entities.Products;

public class WishListTests : ProductCollectionTests
{
    private readonly WishList _sut = WishListBuilder.Valid();

    protected override ProductCollection CreateSut() => new WishList(_sut.UserId, _sut.Name);

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsValid_ThenCreatesCart()
    {
        var result = WishList.Create(_sut.UserId, _sut.Name);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.UserId.Should().Be(_sut.UserId);
        result.Value.Items.Should().BeEmpty();
        result.Value.Name.Should().Be(_sut.Name);
    }

    [Fact]
    public void GivenEntityCreation_WhenUserIsIsEmpty_ThenReturnsFailure()
    {
        var result = WishList.Create(Guid.Empty, _sut.Name);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(WishListErrors.EmptyUserId.Code);
    }

    [Fact]
    public void GivenEntityCreation_WhenNameIsEmpty_ThenReturnsFailure()
    {
        var result = WishList.Create(_sut.UserId, "");

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(WishListErrors.EmptyName.Code);
    }
}
