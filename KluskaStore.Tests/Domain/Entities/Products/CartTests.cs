using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Tests.Domain.Entities.Products;

public class CartTests : ProductCollectionTests
{
    protected override ProductCollection CreateSut(IEnumerable<Item> items) => new Cart(Guid.NewGuid(), items);

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsValid_ThenCreatesCart()
    {
        var userId = Guid.NewGuid();
        var result = Cart.Create(userId, Items);

        result.IsSuccess.Should().BeTrue();
        result.Value.UserId.Should().Be(userId);
        result.Value.Items.Should().BeEquivalentTo(Items.Where(i => i.Quantity > 0));
    }

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsInvalid_ThenReturnsFailure()
    {
        var result = Cart.Create(Guid.Empty, []);

        result.IsFailure.Should().BeTrue();
    }
}
