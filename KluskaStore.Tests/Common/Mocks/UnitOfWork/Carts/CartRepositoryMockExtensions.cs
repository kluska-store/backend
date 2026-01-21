using System.Runtime.CompilerServices;
using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Tests.Common.Mocks.UnitOfWork.Carts;

public static class CartRepositoryMockExtensions
{
    public static void VerifyGetCartByUserIdCalled(this Mock<IUnitOfWork> mock, Func<Times> times) => mock.Verify(
        uow => uow.Carts.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
        times
    );

    public static void VerifyAddCartCalled(this Mock<IUnitOfWork> mock, Func<Times> times) => mock.Verify(
        uow => uow.Carts.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()),
        times
    );

    public static void SetupGetCartByUserIdReturnsNull(this Mock<IUnitOfWork> mock) => mock
        .Setup(uow => uow.Carts.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Cart?)null);

    public static void SetupGetCartByUserIdReturns(this Mock<IUnitOfWork> mock, Cart cart) => mock
        .Setup(uow => uow.Carts.GetByUserIdAsync(cart.UserId, It.IsAny<CancellationToken>()))
        .ReturnsAsync(cart);
}
