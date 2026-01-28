using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Tests.Common.Mocks.UnitOfWork.Items;

public static class ItemRepositoryMockExtensions
{
    public static void SetupGetItemByIdReturns(this Mock<IUnitOfWork> mock, Item item) => mock
        .Setup(uow => uow.Items.GetByIdAsync(item.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(item);

    public static void SetupGetItemByIdReturnsNull(this Mock<IUnitOfWork> mock) => mock
        .Setup(uow => uow.Items.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Item?)null);

    public static void VerifyGetItemByIdCalled(this Mock<IUnitOfWork> mock, Func<Times> times) => mock.Verify(
        uow => uow.Items.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
        times
    );
}
