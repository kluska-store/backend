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
    public void GivenItemUpdate_WhenItemDoesNotExist_ThenCreatesItem()
    {
        uint quantity = 20;
        var sut = CreateSut();
        var result = sut.SetItem(Products[3], quantity);

        var item = sut.Items.ToList().Find(i => i.Product == Products[3]);
        item.Should().NotBeNull();
        item.Quantity.Should().Be(quantity);
        sut.Items.Count.Should().Be(4);
        result.Should().BeTrue();
    }

    [Fact]
    public void GivenItemUpdate_WhenItemExistsAndFinalQuantityIsGraterThenZero_ThenUpdatesItemQuantity()
    {
        uint newQuantity = 2;
        var sut = CreateSut();
        var result = sut.SetItem(Products[0], newQuantity);

        var item = sut.Items.ToList().Find(i => i.Product == Products[0]);
        item.Should().BeSameAs(Items[0]);
        item.Quantity.Should().Be(newQuantity);
        result.Should().BeTrue();
    }

    [Fact]
    public void GivenItemUpdate_WhenItemExistisAndFinalQuantityIsLessThanOrEqualToZero_ThenRemovesItem()
    {
        var sut = CreateSut();
        var result = sut.SetItem(Products[0], 0);

        sut.Items.Should().NotContain(Items[0]);
        result.Should().BeFalse();
    }

    [Fact]
    public void GivenItemAddition_WhenItemDoesNotExist_ThenCreatesItem()
    {
        var sut = CreateSut();
        sut.AddItem(Products[3]);

        var item = sut.Items.ToList().Find(i => i.Product == Products[3]);
        item.Should().NotBeNull();
        item.Quantity.Should().Be(1);
    }

    [Fact]
    public void GivenItemAddition_WhenItemAlreadyExists_ThenItemQuantityIncreasesInOne()
    {
        var lastQuantity = Items[0].Quantity;
        var sut = CreateSut();
        sut.AddItem(Products[0]);

        var item = sut.Items.ToList().Find(i => i.Product == Products[0]);
        item.Should().BeSameAs(Items[0]);
        item.Quantity.Should().Be(lastQuantity + 1);
    }

    [Fact]
    public void GivenItemRemoval_ThenRemovesItem()
    {
        var sut = CreateSut();
        sut.RemoveItem(Products[0]);

        sut.Items.Should().NotContain(Items[0]);
    }
}
