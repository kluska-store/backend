using KluskaStore.Domain.Entities.Products;
using KluskaStore.Domain.Errors.Entities;

namespace KluskaStore.Tests.Domain.Entities.Products;

public class CartTests : ProductCollectionTests
{
    private readonly Cart _sut = new(Guid.NewGuid(), [new Item(new Product(10, "product"), 2)]);
    protected override ProductCollection CreateSut(IEnumerable<Item> items) => new Cart(_sut.UserId, items);

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsValid_ThenCreatesCart()
    {
        var result = Cart.Create(_sut.UserId, _sut.Items);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.UserId.Should().Be(_sut.UserId);
        result.Value.Items.Should().BeEquivalentTo(_sut.Items.Where(i => i.Quantity > 0));
    }

    [Fact]
    public void GivenEntityCreation_WhenUserIsIsEmpty_ThenReturnsFailure()
    {
        var result = Cart.Create(Guid.Empty, _sut.Items);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(CartErrors.EmptyUserId.Code);
    }
}
