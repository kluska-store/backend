using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Tests.Common.Mocks.UnitOfWork.Products;

public static class ProductRepositoryMockExtensions
{
    public static void SetupGetProductByIdReturns(this Mock<IUnitOfWork> mock, Product product) => mock
        .Setup(uow => uow.Products.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(product);

    public static void SetupGetProductByIdReturnsNull(this Mock<IUnitOfWork> mock) => mock
        .Setup(uow => uow.Products.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Product?)null);

    public static void VerifyGetProductByIdCalled(this Mock<IUnitOfWork> mock, Func<Times> times) => mock.Verify(
        uow => uow.Products.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
        times
    );
}
