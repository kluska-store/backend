using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Tests.Common.Mocks.Products;

public static class ProductRepositoryMockExtensions
{
    public static void SetupGetProductByIdReturns(this Mock<IProductRepository> mock, Product product) => mock
        .Setup(repo => repo.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(product);

    public static void SetupGetProductByIdReturnsNull(this Mock<IProductRepository> mock) => mock
        .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Product?)null);

    public static void VerifyGetProductByIdCalled(this Mock<IProductRepository> mock, Func<Times> times) => mock.Verify(
        repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
        times
    );
}
