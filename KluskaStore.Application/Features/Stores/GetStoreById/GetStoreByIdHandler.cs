using KluskaStore.Application.Interfaces.Stores;
using KluskaStore.Domain.Repositories;
using KluskaStore.Domain.Shared;

namespace KluskaStore.Application.Features.Stores.GetStoreById;

public class GetStoreByIdHandler(IUnitOfWork uow) :
  Handler<GetStoreByIdCommand, Result<StoreResponse>>(uow),
  IGetStoreByIdHandler {
  public override async Task<Result<StoreResponse>> Handle(GetStoreByIdCommand request, CancellationToken cancellationToken) {
    var store = await UoW.Stores.GetByIdAsync(request.StoreId);

    return store is null
      ? Result<StoreResponse>.Failure($"Store with id {request.StoreId} not found")
      : Result<StoreResponse>.Success(new StoreResponse(
        store.Id,
        store.Cnpj.Value,
        store.Name,
        store.Email.Value,
        store.IsActive
      ));
  }
}