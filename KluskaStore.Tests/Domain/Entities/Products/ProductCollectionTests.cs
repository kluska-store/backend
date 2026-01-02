using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Tests.Domain.Entities.Products;

public abstract class ProductCollectionTests
{
    protected readonly List<Item> Items;
    protected readonly List<Product> Products =
    [
        new(10, "p1"),
        new(20, "p2"),
        new(30, "p3"),
        new(40, "p4")
    ];

    protected ProductCollectionTests() => Items =
    [
        new Item(Products[0], 10),
        new Item(Products[1], 5),
        new Item(Products[2], 15)
    ];

    protected abstract ProductCollection CreateSut(IEnumerable<Item> items);
    private ProductCollection CreateSut() => CreateSut(Items);

    [Fact]
    public void GivenTotalPriceCalculation_ThenCalculatesTotalPrice()
    {
        var sut = CreateSut();
        sut.CalculateTotalPrice().Should().Be(sut.Items.Select(i => i.Product.Price * i.Quantity).Sum());
    }

    [Fact]
    public void GivenItemAddition_WhenItemDoesNotExist_ThenCreatesItem()
    {
        var sut = CreateSut();
        var item = new Item(Products[3], 30);
        var lastCount = sut.Items.Count;

        sut.AddItem(item);

        var stored = sut.Items.FirstOrDefault(i => i == item);
        stored.Should().NotBeNull();
        stored.Quantity.Should().Be(item.Quantity);
        sut.Items.Count.Should().Be(lastCount + 1);
    }

    [Fact]
    public void GivenItemAddition_WhenItemAlreadyExists_ThenIncreasesItemsQuantity()
    {
        var sut = CreateSut();
        var item = new Item(Products[0], 15);
        var stored = sut.Items.First(i => i == item);
        var lastItemCount = sut.Items.Count;
        var expectedFinalQuantity = item.Quantity + stored.Quantity;

        sut.AddItem(item);

        sut.Items.First(i => i == item).Quantity.Should().Be(expectedFinalQuantity);
        sut.Items.Count.Should().Be(lastItemCount);
    }

    [Fact]
    public void GivenItemRemoval_WhenItemDoesNotExist_ThenDoesNothing()
    {
        var sut = CreateSut();
        var item = new Item(Products[3], 30);
        var lastCount = sut.Items.Count;

        sut.RemoveItem(item);

        sut.Items.Count.Should().Be(lastCount);
    }

    [Fact]
    public void GivenItemRemoval_WhenItemAlreadyExists_ThenRemovesItem()
    {
        var sut = CreateSut();
        var item = Items[0];
        var lastCount = sut.Items.Count;

        sut.RemoveItem(item);

        sut.Items.Count.Should().Be(lastCount - 1);
        sut.Items.Should().NotContain(item);
    }
}
