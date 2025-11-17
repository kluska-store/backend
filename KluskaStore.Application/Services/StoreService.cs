using KluskaStore.Application.Features.Stores;
using KluskaStore.Application.Features.Stores.CreateStore;
using KluskaStore.Application.Features.Stores.GetStoreById;
using KluskaStore.Application.Interfaces.Stores;
using KluskaStore.Domain.Repositories;
using KluskaStore.Domain.Shared;

namespace KluskaStore.Application.Services;

public class StoreService : Service, IStoreService {
  private readonly ICreateStoreHandler _createStoreHandler;
  private readonly IGetStoreByIdHandler _getStoreByIdHandler;

  public StoreService(
    IUnitOfWork uow,
    ICreateStoreHandler createStoreHandler,
    IGetStoreByIdHandler getStoreByIdHandler
  ) : base(uow) {
    _createStoreHandler = createStoreHandler;
    _getStoreByIdHandler = getStoreByIdHandler;
  }

  public async Task<Result<StoreResponse>> GetStoreByIdAsync(
    GetStoreByIdCommand request,
    CancellationToken cancellationToken = default
  ) {
    return await _getStoreByIdHandler.Handle(request, cancellationToken);
  }

  public async Task<Result<StoreResponse>> CreateStoreAsync(
    CreateStoreCommand request,
    CancellationToken cancellationToken = default
  ) {
    return await _createStoreHandler.Handle(request, cancellationToken);
  }
}