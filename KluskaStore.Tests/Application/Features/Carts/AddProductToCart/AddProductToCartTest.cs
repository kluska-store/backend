using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Application.Features.Carts.AddProductToCart;
using KluskaStore.Domain.Entities.Accounts;
using KluskaStore.Domain.Entities.Products;
using KluskaStore.Domain.Errors.Entities;
using KluskaStore.Tests.Common.Builders;
using KluskaStore.Tests.Common.Mocks.UnitOfWork;
using KluskaStore.Tests.Common.Mocks.UnitOfWork.Carts;
using KluskaStore.Tests.Common.Mocks.UnitOfWork.Products;
using KluskaStore.Tests.Common.Mocks.UnitOfWork.Sessions;

namespace KluskaStore.Tests.Application.Features.Carts.AddProductToCart;

public sealed class AddProductToCartTest
{
    private readonly Mock<IUnitOfWork> _mock = new();
    private readonly AddProductToCartHandler _sut;

    public AddProductToCartTest() => _sut = new AddProductToCartHandler(_mock.Object);

    private static AddProductToCartCommand GenerateValidCommand(string? sessionToken = null, Guid? productId = null, uint? quantity = null) =>
        new(sessionToken ?? "token", productId ?? Guid.NewGuid(), quantity ?? 1);

    [Fact]
    public async Task GivenUserNotLoggedIn_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        _mock.SetupGetSessionByTokenReturnsNull();
        var command = GenerateValidCommand();

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Should().Be(AddProductToCartErrors.NotLoggedIn);
        _mock.VerifyGetSessionByTokenCalled(Times.Once);
        _mock.VerifyGetProductByIdCalled(Times.Never);
        _mock.VerifyGetCartByUserIdCalled(Times.Never);
        _mock.VerifyAddCartCalled(Times.Never);
        _mock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenUserWithExpiredSession_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        _mock.SetupGetSessionByTokenReturns(SessionBuilder.Expired(isUserSession: true));
        var command = GenerateValidCommand();

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Should().Be(AddProductToCartErrors.NotLoggedIn);
        _mock.VerifyGetSessionByTokenCalled(Times.Once);
        _mock.VerifyGetProductByIdCalled(Times.Never);
        _mock.VerifyGetCartByUserIdCalled(Times.Never);
        _mock.VerifyAddCartCalled(Times.Never);
        _mock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenValidStoreSession_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        var session = SessionBuilder.Valid(isUserSession: false);
        var command = GenerateValidCommand(sessionToken: session.Token);
        _mock.SetupGetSessionByTokenReturns(session);

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Should().Be(AddProductToCartErrors.NotAnUser);
        _mock.VerifyGetSessionByTokenCalled(Times.Once);
        _mock.VerifyGetProductByIdCalled(Times.Never);
        _mock.VerifyGetCartByUserIdCalled(Times.Never);
        _mock.VerifyAddCartCalled(Times.Never);
        _mock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenNonExistingProduct_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        var session = SessionBuilder.Valid(isUserSession: true);
        var command = GenerateValidCommand(sessionToken: session.Token);
        _mock.SetupGetSessionByTokenReturns(session);
        _mock.SetupGetProductByIdReturnsNull();

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Should().Be(AddProductToCartErrors.ProductNotFound);
        _mock.VerifyGetSessionByTokenCalled(Times.Once);
        _mock.VerifyGetProductByIdCalled(Times.Once);
        _mock.VerifyGetCartByUserIdCalled(Times.Never);
        _mock.VerifyAddCartCalled(Times.Never);
        _mock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenNonAvailableProduct_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        var queriedProduct = ProductBuilder.Unavailable();
        var session = SessionBuilder.Valid(isUserSession: true);
        var command = GenerateValidCommand(productId: queriedProduct.Id, sessionToken: session.Token);
        _mock.SetupGetSessionByTokenReturns(session);
        _mock.SetupGetProductByIdReturns(queriedProduct);

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Should().Be(AddProductToCartErrors.ProductNotAvailable);
        _mock.VerifyGetSessionByTokenCalled(Times.Once);
        _mock.VerifyGetProductByIdCalled(Times.Once);
        _mock.VerifyGetCartByUserIdCalled(Times.Never);
        _mock.VerifyAddCartCalled(Times.Never);
        _mock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenItemQuantityEqualToZero_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        var queriedProduct = ProductBuilder.Valid();
        var session = SessionBuilder.Valid(isUserSession: true);
        var command = GenerateValidCommand(productId: queriedProduct.Id, quantity: 0, sessionToken: session.Token);
        _mock.SetupGetSessionByTokenReturns(session);
        _mock.SetupGetProductByIdReturns(queriedProduct);

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Should().Be(ItemErrors.EmptyItem);
        _mock.VerifyGetSessionByTokenCalled(Times.Once);
        _mock.VerifyGetProductByIdCalled(Times.Once);
        _mock.VerifyGetCartByUserIdCalled(Times.Never);
        _mock.VerifyAddCartCalled(Times.Never);
        _mock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenNonExistingCart_WhenTryingToAddProductToCart_ThenReturnsFailure()
    {
        var queriedProduct = ProductBuilder.Valid();
        var session = SessionBuilder.Valid(isUserSession: true);
        var command = GenerateValidCommand(productId: queriedProduct.Id, sessionToken: session.Token);
        Cart? persistedCart = null;
        _mock.SetupGetSessionByTokenReturns(session);
        _mock.SetupGetProductByIdReturns(queriedProduct);
        _mock.SetupGetCartByUserIdReturnsNull();
        _mock.Setup(uow => uow.Carts.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
            .Callback<Cart, CancellationToken>((cart, _) => persistedCart = cart);

        var result = await _sut.Handle(command);

        result.IsSuccess.Should().BeTrue();
        persistedCart.Should().NotBeNull();
        persistedCart.Items.Should().HaveCount(1);
        persistedCart.Items.Single().Quantity.Should().Be(command.Quantity);
        persistedCart.Items.Single().Product.Id.Should().Be(command.ProductId);
        _mock.VerifyGetSessionByTokenCalled(Times.Once);
        _mock.VerifyGetProductByIdCalled(Times.Once);
        _mock.VerifyGetCartByUserIdCalled(Times.Once);
        _mock.VerifyAddCartCalled(Times.Once);
        _mock.VerifyCommitAsyncCalled(Times.Once);
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
        _mock.SetupGetSessionByTokenReturns(session);
        _mock.SetupGetProductByIdReturns(storedItem.Product);
        _mock.SetupGetCartByUserIdReturns(persistedCart);

        var result = await _sut.Handle(command);

        result.IsSuccess.Should().BeTrue();
        persistedCart.Should().NotBeNull();
        persistedCart.Items.Should().HaveCount(1);
        persistedCart.Items.Single().Quantity.Should().Be(expectedFinalQuantity);
        persistedCart.Items.Single().Product.Id.Should().Be(storedItem.Product.Id);
        _mock.VerifyGetSessionByTokenCalled(Times.Once);
        _mock.VerifyGetProductByIdCalled(Times.Once);
        _mock.VerifyGetCartByUserIdCalled(Times.Once);
        _mock.VerifyAddCartCalled(Times.Never);
        _mock.VerifyCommitAsyncCalled(Times.Once);
    }
}
