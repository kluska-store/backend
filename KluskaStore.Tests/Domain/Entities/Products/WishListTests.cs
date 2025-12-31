using KluskaStore.Domain.Entities.Products;
using KluskaStore.Domain.Errors.Entities;

namespace KluskaStore.Tests.Domain.Entities.Products;

public class WishListTests : ProductCollectionTests
{
    private readonly WishList _sut = new(Guid.NewGuid(), [new Item(new Product(10, "product"), 10)], "wish list");

    protected override ProductCollection CreateSut(IEnumerable<Item> items) =>
        new WishList(_sut.UserId, items, _sut.Name);

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsValid_ThenCreatesCart()
    {
        var result = WishList.Create(_sut.UserId, Items, _sut.Name);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.UserId.Should().Be(_sut.UserId);
        result.Value.Items.Should().BeEquivalentTo(Items.Where(i => i.Quantity > 0));
        result.Value.Name.Should().Be(_sut.Name);
    }

    [Fact]
    public void GivenEntityCreation_WhenUserIsIsEmpty_ThenReturnsFailure()
    {
        var result = WishList.Create(Guid.Empty, _sut.Items, _sut.Name);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(WishListErrors.EmptyUserId.Code);
    }

    [Fact]
    public void GivenEntityCreation_WhenNameIsEmpty_ThenReturnsFailure()
    {
        var result = WishList.Create(_sut.UserId, _sut.Items, "");

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(WishListErrors.EmptyName.Code);
    }
}
