using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Application.Features.Products.GetProductById;
using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Tests.Application.Features.Products.GetProductById;

public class GetProductByIdTests
{
    private readonly Mock<IProductRepository> _mock = new();
    private readonly GetProductByIdHandler _sut;

    public GetProductByIdTests() => _sut = new GetProductByIdHandler(_mock.Object);

    private static Product GenerateValidProduct(bool isAvailable = true)
    {
        var product = new Product(
            new Dictionary<string, string>(),
            100,
            "Sac of Rice"
        );

        if (!isAvailable) product.MarkAsUnavailable();
        return product;
    }

    private static GetProductByIdQuery GenerateValidQuery() => new(Guid.NewGuid());

    private void SetupGetProductByIdReturnsNull() => _mock
        .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Product?)null);

    private void SetupGetProductByIdReturnsUnavailableProduct() => _mock
        .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(GenerateValidProduct(isAvailable: false));

    private void SetupGetProductByIdReturnsAvailableProduct() => _mock
        .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(GenerateValidProduct());

    private void VerifyGetProductByIdCalledOnce() => _mock.Verify(
        repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
        Times.Once
    );

    [Fact]
    public async Task GivenNonExistingProduct_WhenGettingById_ThenReturnsFailure()
    {
        SetupGetProductByIdReturnsNull();
        var query = GenerateValidQuery();

        var result = await _sut.Handle(query);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(GetProductByIdErrors.ProductNotFound.Code);
    }

    [Fact]
    public async Task GivenUnavailableProduct_WhenGettingById_ThenReturnsFailure()
    {
        SetupGetProductByIdReturnsUnavailableProduct();
        var query = GenerateValidQuery();

        var result = await _sut.Handle(query);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(GetProductByIdErrors.ProductIsUnavailable.Code);
    }

    [Fact]
    public async Task GivenAvailableProduct_WhenGettingById_ThenReturnsProductDto()
    {
        SetupGetProductByIdReturnsAvailableProduct();
        var query = GenerateValidQuery();

        var result = await _sut.Handle(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }
}
