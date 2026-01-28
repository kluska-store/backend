using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Application.Features.Carts.RemoveItemFromCart;
using KluskaStore.Tests.Common.Builders;
using KluskaStore.Tests.Common.Mocks.UnitOfWork;
using KluskaStore.Tests.Common.Mocks.UnitOfWork.Carts;
using KluskaStore.Tests.Common.Mocks.UnitOfWork.Items;
using KluskaStore.Tests.Common.Mocks.UnitOfWork.Sessions;

namespace KluskaStore.Tests.Application.Features.Carts.RemoveItemFromCart;

public sealed class RemoveItemFromCartTests
{
    private readonly Mock<IUnitOfWork> _mock = new();
    private readonly RemoveItemFromCartHandler _sut;

    public RemoveItemFromCartTests() => _sut = new RemoveItemFromCartHandler(_mock.Object);

    private RemoveItemFromCartCommand GenerateValidCommand(string? sessionToken = null, Guid? itemId = null)
        => new(sessionToken ?? "session token", itemId ?? Guid.NewGuid());

    [Fact]
    public async Task GivenUserNotLoggedIn_WhenTryingToRemoveItemFromCart_ThenReturnsFailure()
    {
        var command = GenerateValidCommand();
        _mock.SetupGetSessionByTokenReturnsNull();

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(RemoveItemFromCartErrors.NotLoggedIn.Code);
        _mock.VerifyGetSessionByTokenCalled(Times.Once);
        _mock.VerifyGetCartByUserIdCalled(Times.Never);
        _mock.VerifyGetItemByIdCalled(Times.Never);
        _mock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenUserWithExpiredSession_WhenTryingToRemoveItemFromCart_ThenReturnsFailure()
    {
        var session = SessionBuilder.Expired(isUserSession: true);
        var command = GenerateValidCommand(sessionToken: session.Token);
        _mock.SetupGetSessionByTokenReturns(session);

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(RemoveItemFromCartErrors.NotLoggedIn.Code);
        _mock.VerifyGetSessionByTokenCalled(Times.Once);
        _mock.VerifyGetCartByUserIdCalled(Times.Never);
        _mock.VerifyGetItemByIdCalled(Times.Never);
        _mock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenStoreAccountLoggedIn_WhenTryingToRemoveItemFromCart_ThenReturnsFailure()
    {
        var session = SessionBuilder.Valid(isUserSession: false);
        var command = GenerateValidCommand(sessionToken: session.Token);
        _mock.SetupGetSessionByTokenReturns(session);

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(RemoveItemFromCartErrors.NotAnUser.Code);
        _mock.VerifyGetSessionByTokenCalled(Times.Once);
        _mock.VerifyGetCartByUserIdCalled(Times.Never);
        _mock.VerifyGetItemByIdCalled(Times.Never);
        _mock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenUserWithoutExistingCart_WhenTryingToRemoveItemFromCart_ThenReturnsFailure()
    {
        var session = SessionBuilder.Valid(isUserSession: true);
        var command = GenerateValidCommand(sessionToken: session.Token);
        _mock.SetupGetSessionByTokenReturns(session);
        _mock.SetupGetCartByUserIdReturnsNull();

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(RemoveItemFromCartErrors.CartNotFound.Code);
        _mock.VerifyGetSessionByTokenCalled(Times.Once);
        _mock.VerifyGetCartByUserIdCalled(Times.Once);
        _mock.VerifyGetItemByIdCalled(Times.Never);
        _mock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenNonExistingItem_WhenTryingToRemoveItemFromCart_ThenReturnsFailure()
    {
        var session = SessionBuilder.Valid(isUserSession: true);
        var cart = CartBuilder.WithUserId(session.Owner.OwnerId);
        var command = GenerateValidCommand(sessionToken: session.Token);
        _mock.SetupGetSessionByTokenReturns(session);
        _mock.SetupGetCartByUserIdReturns(cart);
        _mock.SetupGetItemByIdReturnsNull();

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(RemoveItemFromCartErrors.ItemNotFound.Code);
        _mock.VerifyGetSessionByTokenCalled(Times.Once);
        _mock.VerifyGetCartByUserIdCalled(Times.Once);
        _mock.VerifyGetItemByIdCalled(Times.Once);
        _mock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenItemNotStoredInCart_WhenTryingToRemoveItemFromCart_ThenReturnsFailure()
    {
        var session = SessionBuilder.Valid(isUserSession: true);
        var cart = CartBuilder.WithUserId(session.Owner.OwnerId);
        var item = ItemBuilder.Valid();
        var command = GenerateValidCommand(sessionToken: session.Token, itemId: item.Id);
        _mock.SetupGetSessionByTokenReturns(session);
        _mock.SetupGetCartByUserIdReturns(cart);
        _mock.SetupGetItemByIdReturns(item);

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(RemoveItemFromCartErrors.ItemNotInCart.Code);
        _mock.VerifyGetSessionByTokenCalled(Times.Once);
        _mock.VerifyGetCartByUserIdCalled(Times.Once);
        _mock.VerifyGetItemByIdCalled(Times.Once);
        _mock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenItemStoredInExistingCart_WhenTryingToRemoveItemFromCart_ThenReturnsSuccess()
    {
        var session = SessionBuilder.Valid(isUserSession: true);
        var cart = CartBuilder.WithUserId(session.Owner.OwnerId);
        var item = ItemBuilder.Valid();
        var command = GenerateValidCommand(sessionToken: session.Token, itemId: item.Id);
        cart.AddItem(item);
        _mock.SetupGetSessionByTokenReturns(session);
        _mock.SetupGetCartByUserIdReturns(cart);
        _mock.SetupGetItemByIdReturns(item);

        var result = await _sut.Handle(command);

        result.IsSuccess.Should().BeTrue();
        _mock.VerifyGetSessionByTokenCalled(Times.Once);
        _mock.VerifyGetCartByUserIdCalled(Times.Once);
        _mock.VerifyGetItemByIdCalled(Times.Once);
        _mock.VerifyCommitAsyncCalled(Times.Once);
    }
}
