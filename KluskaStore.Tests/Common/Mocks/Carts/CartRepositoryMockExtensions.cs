using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Tests.Common.Mocks.Carts;

public static class CartRepositoryMockExtensions
{
    public static void VerifyGetCartByUserIdCalled(this Mock<ICartRepository> mock, Func<Times> times) => mock.Verify(
        repo => repo.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
        times
    );

    public static void VerifyAddCartCalled(this Mock<ICartRepository> mock, Func<Times> times) => mock.Verify(
        repo => repo.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()),
        times
    );

    public static void SetupGetCartByUserIdReturnsNull(this Mock<ICartRepository> mock) => mock
        .Setup(repo => repo.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Cart?)null);

    public static void SetupGetCartByUserIdReturns(this Mock<ICartRepository> mock, Cart cart) => mock
        .Setup(repo => repo.GetByUserIdAsync(cart.UserId, It.IsAny<CancellationToken>()))
        .ReturnsAsync(cart);
}
