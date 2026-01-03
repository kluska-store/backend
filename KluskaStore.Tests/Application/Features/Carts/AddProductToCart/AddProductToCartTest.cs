using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Application.Features.Carts.AddProductToCart;
using KluskaStore.Domain.Entities.Accounts;
using KluskaStore.Domain.Entities.Products;
using KluskaStore.Domain.Errors.Entities;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Tests.Application.Features.Carts.AddProductToCart;

public sealed class AddProductToCartTest
{
    private readonly Mock<IUnitOfWork> _mock = new();
    private readonly AddProductToCartHandler _sut;

    public AddProductToCartTest() => _sut = new AddProductToCartHandler(_mock.Object);

    private static AddProductToCartCommand GenerateValidCommand(Guid? productId = null, uint? quantity = null) =>
        new("token", productId ?? Guid.NewGuid(), quantity ?? 1);

    private static SessionOwner GenerateUserSessionOwner() => new(SessionOwner.OwnerTypeEnum.User, Guid.NewGuid());
    private static SessionOwner GenerateStoreSessionOwner() => new(SessionOwner.OwnerTypeEnum.Store, Guid.NewGuid());

    private static Session GenerateValidSession(
        SessionOwner? owner = null,
        DateTime? createdAt = null
    ) => new("token", owner ?? GenerateUserSessionOwner(), createdAt ?? DateTime.UtcNow);

    private static Product GenerateValidProduct(bool isAvailable = true)
    {
        var product = new Product(10, "product");
        if (!isAvailable) product.MarkAsUnavailable();
        return product;
    }

    private static Cart GenerateEmptyCart() => new(Guid.NewGuid());

    private static Cart GenerateCartWithItem(Item item)
    {
        var cart = GenerateEmptyCart();
        cart.AddItem(item);
        return cart;
    }

    private void SetupGetSessionByTokenReturn(Session? expectedValue) => _mock
        .Setup(uow => uow.Sessions.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(expectedValue);

    private void SetupGetProductByIdReturn(Product? expectedValue)
    {
        var returnedProductId = expectedValue?.Id ?? It.IsAny<Guid>();
        _mock
            .Setup(uow => uow.Products.GetByIdAsync(returnedProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedValue);
    }

    private void SetupGetCartByUserIdReturn(Cart? expectedValue) => _mock
        .Setup(uow => uow.Carts.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(expectedValue);

    private void VerifyGetSessionByTokenCalledOnce() => _mock.Verify(
        uow => uow.Sessions.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
        Times.Once
    );

    private void VerifyGetProductByIdNeverCalled() => _mock.Verify(
        uow => uow.Products.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
        Times.Never
    );

    private void VerifyGetProductByIdCalledOnce() => _mock.Verify(
        uow => uow.Products.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
        Times.Once
    );

    private void VerifyGetCartByUserIdNeverCalled() => _mock.Verify(
        uow => uow.Carts.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
        Times.Never
    );

    private void VerifyGetCartByUserIdCalledOnce() => _mock.Verify(
        uow => uow.Carts.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
        Times.Once
    );

    private void VerifyAddCartNeverCalled() => _mock.Verify(
        uow => uow.Carts.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()),
        Times.Never
    );

    private void VerifyAddCartCalledOnce() => _mock.Verify(
        uow => uow.Carts.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()),
        Times.Once
    );

    private void VerifyCommitAsyncNeverCalled() => _mock.Verify(
        uow => uow.CommitAsync(It.IsAny<CancellationToken>()),
        Times.Never
    );

    private void VerifyCommitAsyncCalledOnce() => _mock.Verify(
        uow => uow.CommitAsync(It.IsAny<CancellationToken>()),
        Times.Once
    );

    [Fact]
    public async Task GivenUserNotLoggedIn_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        SetupGetSessionByTokenReturn(null);
        var command = GenerateValidCommand();

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Should().Be(AddProductToCartErrors.NotLoggedIn);
        VerifyGetSessionByTokenCalledOnce();
        VerifyGetProductByIdNeverCalled();
        VerifyGetCartByUserIdNeverCalled();
        VerifyAddCartNeverCalled();
        VerifyCommitAsyncNeverCalled();
    }

    [Fact]
    public async Task GivenUserWithExpiredSession_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        SetupGetSessionByTokenReturn(GenerateValidSession(createdAt: DateTime.UtcNow.AddYears(-2)));
        var command = GenerateValidCommand();

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Should().Be(AddProductToCartErrors.NotLoggedIn);
        VerifyGetSessionByTokenCalledOnce();
        VerifyGetProductByIdNeverCalled();
        VerifyGetCartByUserIdNeverCalled();
        VerifyAddCartNeverCalled();
        VerifyCommitAsyncNeverCalled();
    }

    [Fact]
    public async Task GivenValidStoreSession_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        SetupGetSessionByTokenReturn(GenerateValidSession(owner: GenerateStoreSessionOwner()));
        var command = GenerateValidCommand();

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Should().Be(AddProductToCartErrors.NotAnUser);
        VerifyGetSessionByTokenCalledOnce();
        VerifyGetProductByIdNeverCalled();
        VerifyGetCartByUserIdNeverCalled();
        VerifyAddCartNeverCalled();
        VerifyCommitAsyncNeverCalled();
    }

    [Fact]
    public async Task GivenNonExistingProduct_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        SetupGetSessionByTokenReturn(GenerateValidSession());
        SetupGetProductByIdReturn(null);
        var command = GenerateValidCommand();

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Should().Be(AddProductToCartErrors.ProductNotFound);
        VerifyGetSessionByTokenCalledOnce();
        VerifyGetProductByIdCalledOnce();
        VerifyGetCartByUserIdNeverCalled();
        VerifyAddCartNeverCalled();
        VerifyCommitAsyncNeverCalled();
    }

    [Fact]
    public async Task GivenNonAvailableProduct_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        var queriedProduct = GenerateValidProduct(isAvailable: false);
        var command = GenerateValidCommand(productId: queriedProduct.Id);
        SetupGetSessionByTokenReturn(GenerateValidSession());
        SetupGetProductByIdReturn(queriedProduct);

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Should().Be(AddProductToCartErrors.ProductNotAvailable);
        VerifyGetSessionByTokenCalledOnce();
        VerifyGetProductByIdCalledOnce();
        VerifyGetCartByUserIdNeverCalled();
        VerifyAddCartNeverCalled();
        VerifyCommitAsyncNeverCalled();
    }

    [Fact]
    public async Task GivenItemQuantityEqualToZero_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        var queriedProduct = GenerateValidProduct();
        var command = GenerateValidCommand(productId: queriedProduct.Id, quantity: 0);
        SetupGetSessionByTokenReturn(GenerateValidSession());
        SetupGetProductByIdReturn(queriedProduct);

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Should().Be(ItemErrors.EmptyItem);
        VerifyGetSessionByTokenCalledOnce();
        VerifyGetProductByIdCalledOnce();
        VerifyGetCartByUserIdNeverCalled();
        VerifyAddCartNeverCalled();
        VerifyCommitAsyncNeverCalled();
    }

    [Fact]
    public async Task GivenNonExistingCart_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        var queriedProduct = GenerateValidProduct();
        var command = GenerateValidCommand(productId: queriedProduct.Id);
        SetupGetSessionByTokenReturn(GenerateValidSession());
        SetupGetProductByIdReturn(queriedProduct);
        Cart? persistedCart = null;
        _mock.Setup(uow => uow.Carts.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
            .Callback<Cart, CancellationToken>((cart, _) => persistedCart = cart);

        var result = await _sut.Handle(command);

        result.IsSuccess.Should().BeTrue();
        persistedCart.Should().NotBeNull();
        persistedCart.Items.Should().HaveCount(1);
        persistedCart.Items.Single().Quantity.Should().Be(command.Quantity);
        persistedCart.Items.Single().Product.Id.Should().Be(command.ProductId);
        VerifyGetSessionByTokenCalledOnce();
        VerifyGetProductByIdCalledOnce();
        VerifyGetCartByUserIdCalledOnce();
        VerifyAddCartCalledOnce();
        VerifyCommitAsyncCalledOnce();
    }

    [Fact]
    public async Task GivenExistingCartContainingAddedProduct_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        var storedItem = new Item(GenerateValidProduct(), 2u);
        var command = GenerateValidCommand(productId: storedItem.Product.Id);
        var expectedFinalQuantity = storedItem.Quantity + command.Quantity;
        var persistedCart = GenerateCartWithItem(storedItem);
        SetupGetSessionByTokenReturn(GenerateValidSession());
        SetupGetProductByIdReturn(storedItem.Product);
        SetupGetCartByUserIdReturn(persistedCart);

        var result = await _sut.Handle(command);

        result.IsSuccess.Should().BeTrue();
        persistedCart.Should().NotBeNull();
        persistedCart.Items.Should().HaveCount(1);
        persistedCart.Items.Single().Quantity.Should().Be(expectedFinalQuantity);
        persistedCart.Items.Single().Product.Id.Should().Be(storedItem.Product.Id);
        VerifyGetSessionByTokenCalledOnce();
        VerifyGetProductByIdCalledOnce();
        VerifyGetCartByUserIdCalledOnce();
        VerifyAddCartNeverCalled();
        VerifyCommitAsyncCalledOnce();
    }
}
