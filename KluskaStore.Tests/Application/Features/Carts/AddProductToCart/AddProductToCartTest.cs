using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Application.Features.Carts.AddProductToCart;
using KluskaStore.Domain.Entities.Products;
using KluskaStore.Domain.Errors.Entities;
using KluskaStore.Tests.Common.Builders;
using KluskaStore.Tests.Common.Mocks.UnitOfWork;
using KluskaStore.Tests.Common.Mocks.Carts;
using KluskaStore.Tests.Common.Mocks.Products;
using KluskaStore.Tests.Common.Mocks.Sessions;

namespace KluskaStore.Tests.Application.Features.Carts.AddProductToCart;

public sealed class AddProductToCartTest
{
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly Mock<ICartRepository> _cartMock = new();
    private readonly Mock<IProductRepository> _productMock = new();
    private readonly Mock<ISessionRepository> _sessionMock = new();
    private readonly AddProductToCartHandler _sut;

    public AddProductToCartTest() => _sut = new AddProductToCartHandler(
        _uowMock.Object,
        _sessionMock.Object,
        _productMock.Object,
        _cartMock.Object
    );

    private static AddProductToCartCommand GenerateValidCommand(string? sessionToken = null, Guid? productId = null,
        uint? quantity = null) =>
        new(sessionToken ?? "token", productId ?? Guid.NewGuid(), quantity ?? 1);

    [Fact]
    public async Task GivenUserNotLoggedIn_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        _sessionMock.SetupGetSessionByTokenReturnsNull();
        var command = GenerateValidCommand();

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Should().Be(AddProductToCartErrors.NotLoggedIn);
        _sessionMock.VerifyGetSessionByTokenCalled(Times.Once);
        _productMock.VerifyGetProductByIdCalled(Times.Never);
        _cartMock.VerifyGetCartByUserIdCalled(Times.Never);
        _cartMock.VerifyAddCartCalled(Times.Never);
        _uowMock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenUserWithExpiredSession_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        _sessionMock.SetupGetSessionByTokenReturns(SessionBuilder.Expired(isUserSession: true));
        var command = GenerateValidCommand();

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Should().Be(AddProductToCartErrors.NotLoggedIn);
        _sessionMock.VerifyGetSessionByTokenCalled(Times.Once);
        _productMock.VerifyGetProductByIdCalled(Times.Never);
        _cartMock.VerifyGetCartByUserIdCalled(Times.Never);
        _cartMock.VerifyAddCartCalled(Times.Never);
        _uowMock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenValidStoreSession_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        var session = SessionBuilder.Valid(isUserSession: false);
        var command = GenerateValidCommand(sessionToken: session.Token);
        _sessionMock.SetupGetSessionByTokenReturns(session);

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Should().Be(AddProductToCartErrors.NotAnUser);
        _sessionMock.VerifyGetSessionByTokenCalled(Times.Once);
        _productMock.VerifyGetProductByIdCalled(Times.Never);
        _cartMock.VerifyGetCartByUserIdCalled(Times.Never);
        _cartMock.VerifyAddCartCalled(Times.Never);
        _uowMock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenNonExistingProduct_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        var session = SessionBuilder.Valid(isUserSession: true);
        var command = GenerateValidCommand(sessionToken: session.Token);
        _sessionMock.SetupGetSessionByTokenReturns(session);
        _productMock.SetupGetProductByIdReturnsNull();

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Should().Be(AddProductToCartErrors.ProductNotFound);
        _sessionMock.VerifyGetSessionByTokenCalled(Times.Once);
        _productMock.VerifyGetProductByIdCalled(Times.Once);
        _cartMock.VerifyGetCartByUserIdCalled(Times.Never);
        _cartMock.VerifyAddCartCalled(Times.Never);
        _uowMock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenNonAvailableProduct_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        var queriedProduct = ProductBuilder.Unavailable();
        var session = SessionBuilder.Valid(isUserSession: true);
        var command = GenerateValidCommand(productId: queriedProduct.Id, sessionToken: session.Token);
        _sessionMock.SetupGetSessionByTokenReturns(session);
        _productMock.SetupGetProductByIdReturns(queriedProduct);

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Should().Be(AddProductToCartErrors.ProductNotAvailable);
        _sessionMock.VerifyGetSessionByTokenCalled(Times.Once);
        _productMock.VerifyGetProductByIdCalled(Times.Once);
        _cartMock.VerifyGetCartByUserIdCalled(Times.Never);
        _cartMock.VerifyAddCartCalled(Times.Never);
    }

    [Fact]
    public async Task GivenItemQuantityEqualToZero_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        var queriedProduct = ProductBuilder.Valid();
        var session = SessionBuilder.Valid(isUserSession: true);
        var command = GenerateValidCommand(productId: queriedProduct.Id, quantity: 0, sessionToken: session.Token);
        _sessionMock.SetupGetSessionByTokenReturns(session);
        _productMock.SetupGetProductByIdReturns(queriedProduct);

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Should().Be(ItemErrors.EmptyItem);
        _sessionMock.VerifyGetSessionByTokenCalled(Times.Once);
        _productMock.VerifyGetProductByIdCalled(Times.Once);
        _cartMock.VerifyGetCartByUserIdCalled(Times.Never);
        _cartMock.VerifyAddCartCalled(Times.Never);
    }

    [Fact]
    public async Task GivenNonExistingCart_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        var queriedProduct = ProductBuilder.Valid();
        var session = SessionBuilder.Valid(isUserSession: true);
        var command = GenerateValidCommand(productId: queriedProduct.Id, sessionToken: session.Token);
        Cart? persistedCart = null;
        _sessionMock.SetupGetSessionByTokenReturns(session);
        _productMock.SetupGetProductByIdReturns(queriedProduct);
        _cartMock.SetupGetCartByUserIdReturnsNull();
        _cartMock.Setup(repo => repo.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
            .Callback<Cart, CancellationToken>((cart, _) => persistedCart = cart);

        var result = await _sut.Handle(command);

        result.IsSuccess.Should().BeTrue();
        persistedCart.Should().NotBeNull();
        persistedCart.Items.Should().HaveCount(1);
        persistedCart.Items.Single().Quantity.Should().Be(command.Quantity);
        persistedCart.Items.Single().Product.Id.Should().Be(command.ProductId);
        _sessionMock.VerifyGetSessionByTokenCalled(Times.Once);
        _productMock.VerifyGetProductByIdCalled(Times.Once);
        _cartMock.VerifyGetCartByUserIdCalled(Times.Once);
        _cartMock.VerifyAddCartCalled(Times.Once);
        _uowMock.VerifyCommitAsyncCalled(Times.Once);
    }

    [Fact]
    public async Task GivenExistingCartContainingAddedProduct_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        var storedItem = ItemBuilder.Valid();
        var session = SessionBuilder.Valid(isUserSession: true);
        var command = GenerateValidCommand(productId: storedItem.Product.Id, sessionToken: session.Token);
        var expectedFinalQuantity = storedItem.Quantity + command.Quantity;
        var persistedCart = CartBuilder.WithUserId(session.Owner.OwnerId);
        persistedCart.AddItem(storedItem);
        _sessionMock.SetupGetSessionByTokenReturns(session);
        _productMock.SetupGetProductByIdReturns(storedItem.Product);
        _cartMock.SetupGetCartByUserIdReturns(persistedCart);

        var result = await _sut.Handle(command);

        result.IsSuccess.Should().BeTrue();
        persistedCart.Should().NotBeNull();
        persistedCart.Items.Should().HaveCount(1);
        persistedCart.Items.Single().Quantity.Should().Be(expectedFinalQuantity);
        persistedCart.Items.Single().Product.Id.Should().Be(storedItem.Product.Id);
        _sessionMock.VerifyGetSessionByTokenCalled(Times.Once);
        _productMock.VerifyGetProductByIdCalled(Times.Once);
        _cartMock.VerifyGetCartByUserIdCalled(Times.Once);
        _cartMock.VerifyAddCartCalled(Times.Never);
        _uowMock.VerifyCommitAsyncCalled(Times.Once);
    }
}
