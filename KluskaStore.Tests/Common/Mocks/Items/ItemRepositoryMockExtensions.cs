using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Tests.Common.Mocks.Items;

public static class ItemRepositoryMockExtensions
{
    public static void SetupGetItemByIdReturns(this Mock<IItemRepository> mock, Item item) => mock
        .Setup(repo => repo.GetByIdAsync(item.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(item);

    public static void SetupGetItemByIdReturnsNull(this Mock<IItemRepository> mock) => mock
        .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Item?)null);

    public static void VerifyGetItemByIdCalled(this Mock<IItemRepository> mock, Func<Times> times) => mock.Verify(
        repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
        times
    );
}
