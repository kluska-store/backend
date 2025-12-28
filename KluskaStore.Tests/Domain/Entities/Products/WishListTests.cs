using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Tests.Domain.Entities.Products;

public class WishListTests : ProductCollectionTests
{
    protected override ProductCollection CreateSut(IEnumerable<Item> items) =>
        new WishList(Guid.NewGuid(), items, "my wishlist");

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsValid_ThenCreatesCart()
    {
        var userId = Guid.NewGuid();
        var name = "things that I am too poor to buy";
        var result = WishList.Create(userId, Items, name);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.UserId.Should().Be(userId);
        result.Value.Items.Should().BeEquivalentTo(Items.Where(i => i.Quantity > 0));
        result.Value.Name.Should().Be(name);
    }

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsInvalid_ThenReturnsFailure()
    {
        var result = WishList.Create(Guid.Empty, [], "");

        result.IsFailure.Should().BeTrue();
    }
}
