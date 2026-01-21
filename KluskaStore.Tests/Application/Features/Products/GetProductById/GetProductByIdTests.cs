using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Application.Features.Products.GetProductById;
using KluskaStore.Tests.Common.Builders;
using KluskaStore.Tests.Common.Mocks.UnitOfWork.Products;

namespace KluskaStore.Tests.Application.Features.Products.GetProductById;

public class GetProductByIdTests
{
    private readonly Mock<IUnitOfWork> _mock = new();
    private readonly GetProductByIdHandler _sut;

    public GetProductByIdTests() => _sut = new GetProductByIdHandler(_mock.Object);

    [Fact]
    public async Task GivenNonExistingProduct_WhenGettingById_ThenReturnsFailure()
    {
        _mock.SetupGetProductByIdReturnsNull();
        var query = new GetProductByIdQuery(Guid.NewGuid());

        var result = await _sut.Handle(query);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(GetProductByIdErrors.ProductNotFound.Code);
        _mock.VerifyGetProductByIdCalled(Times.Once);
    }

    [Fact]
    public async Task GivenUnavailableProduct_WhenGettingById_ThenReturnsFailure()
    {
        var product = ProductBuilder.Unavailable();
        var query = new GetProductByIdQuery(product.Id);
        _mock.SetupGetProductByIdReturns(product);

        var result = await _sut.Handle(query);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(GetProductByIdErrors.ProductIsUnavailable.Code);
        _mock.VerifyGetProductByIdCalled(Times.Once);
    }

    [Fact]
    public async Task GivenAvailableProduct_WhenGettingById_ThenReturnsProductDto()
    {
        var product = ProductBuilder.Valid();
        var query = new GetProductByIdQuery(product.Id);
        _mock.SetupGetProductByIdReturns(product);

        var result = await _sut.Handle(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        _mock.VerifyGetProductByIdCalled(Times.Once);
    }
}
