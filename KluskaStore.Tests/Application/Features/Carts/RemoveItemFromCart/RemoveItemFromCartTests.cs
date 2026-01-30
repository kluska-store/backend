using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Application.Features.Carts.RemoveItemFromCart;
using KluskaStore.Tests.Common.Builders;
using KluskaStore.Tests.Common.Mocks.UnitOfWork;
using KluskaStore.Tests.Common.Mocks.Carts;
using KluskaStore.Tests.Common.Mocks.Sessions;

namespace KluskaStore.Tests.Application.Features.Carts.RemoveItemFromCart;

public sealed class RemoveItemFromCartTests
{
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly Mock<ISessionRepository> _sessionMock = new();
    private readonly Mock<ICartRepository> _cartMock = new();
    private readonly RemoveItemFromCartHandler _sut;

    public RemoveItemFromCartTests() => _sut = new RemoveItemFromCartHandler(
        _uowMock.Object,
        _sessionMock.Object,
        _cartMock.Object
    );

    private RemoveItemFromCartCommand GenerateValidCommand(string? sessionToken = null, Guid? itemId = null)
        => new(sessionToken ?? "session token", itemId ?? Guid.NewGuid());

    [Fact]
    public async Task GivenUserNotLoggedIn_WhenTryingToRemoveItemFromCart_ThenReturnsFailure()
    {
        var command = GenerateValidCommand();
        _sessionMock.SetupGetSessionByTokenReturnsNull();

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(RemoveItemFromCartErrors.NotLoggedIn.Code);
        _sessionMock.VerifyGetSessionByTokenCalled(Times.Once);
        _cartMock.VerifyGetCartByUserIdCalled(Times.Never);
        _uowMock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenUserWithExpiredSession_WhenTryingToRemoveItemFromCart_ThenReturnsFailure()
    {
        var session = SessionBuilder.Expired(isUserSession: true);
        var command = GenerateValidCommand(sessionToken: session.Token);
        _sessionMock.SetupGetSessionByTokenReturns(session);

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(RemoveItemFromCartErrors.NotLoggedIn.Code);
        _sessionMock.VerifyGetSessionByTokenCalled(Times.Once);
        _cartMock.VerifyGetCartByUserIdCalled(Times.Never);
        _uowMock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenStoreAccountLoggedIn_WhenTryingToRemoveItemFromCart_ThenReturnsFailure()
    {
        var session = SessionBuilder.Valid(isUserSession: false);
        var command = GenerateValidCommand(sessionToken: session.Token);
        _sessionMock.SetupGetSessionByTokenReturns(session);

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(RemoveItemFromCartErrors.NotAnUser.Code);
        _sessionMock.VerifyGetSessionByTokenCalled(Times.Once);
        _cartMock.VerifyGetCartByUserIdCalled(Times.Never);
        _uowMock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenUserWithoutExistingCart_WhenTryingToRemoveItemFromCart_ThenReturnsFailure()
    {
        var session = SessionBuilder.Valid(isUserSession: true);
        var command = GenerateValidCommand(sessionToken: session.Token);
        _sessionMock.SetupGetSessionByTokenReturns(session);
        _cartMock.SetupGetCartByUserIdReturnsNull();

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(RemoveItemFromCartErrors.CartNotFound.Code);
        _sessionMock.VerifyGetSessionByTokenCalled(Times.Once);
        _cartMock.VerifyGetCartByUserIdCalled(Times.Once);
        _uowMock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenItemNotStoredInCart_WhenTryingToRemoveItemFromCart_ThenReturnsFailure()
    {
        var session = SessionBuilder.Valid(isUserSession: true);
        var cart = CartBuilder.WithUserId(session.Owner.OwnerId);
        var item = ItemBuilder.Valid();
        var command = GenerateValidCommand(sessionToken: session.Token, itemId: item.Id);
        _sessionMock.SetupGetSessionByTokenReturns(session);
        _cartMock.SetupGetCartByUserIdReturns(cart);

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(RemoveItemFromCartErrors.ItemNotInCart.Code);
        _sessionMock.VerifyGetSessionByTokenCalled(Times.Once);
        _cartMock.VerifyGetCartByUserIdCalled(Times.Once);
        _uowMock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenItemStoredInExistingCart_WhenTryingToRemoveItemFromCart_ThenReturnsSuccess()
    {
        var session = SessionBuilder.Valid(isUserSession: true);
        var cart = CartBuilder.WithUserId(session.Owner.OwnerId);
        var item = ItemBuilder.Valid();
        var command = GenerateValidCommand(sessionToken: session.Token, itemId: item.Id);
        cart.AddItem(item);
        _sessionMock.SetupGetSessionByTokenReturns(session);
        _cartMock.SetupGetCartByUserIdReturns(cart);

        var result = await _sut.Handle(command);

        result.IsSuccess.Should().BeTrue();
        _sessionMock.VerifyGetSessionByTokenCalled(Times.Once);
        _cartMock.VerifyGetCartByUserIdCalled(Times.Once);
        _uowMock.VerifyCommitAsyncCalled(Times.Once);
    }
}
