using KluskaStore.Application.Abstractions;
using KluskaStore.Application.Interfaces.Stores;
using KluskaStore.Domain.Entities;
using KluskaStore.Domain.Repositories;
using KluskaStore.Domain.Shared;

namespace KluskaStore.Application.Features.Stores.GetStoreById;

public class GetStoreByIdHandler(IUnitOfWork uow) :
  Handler<GuidQuery<StoreResponse>, Result<StoreResponse>>(uow),
  IGetStoreByIdHandler {
  public override async Task<Result<StoreResponse>> Handle(GuidQuery<StoreResponse> request, CancellationToken cancellationToken = default) {
    var store = await UoW.Stores.GetByIdAsync(request.Guid);

    return store is null
      ? Result<StoreResponse>.Failure($"Store with id {request.Guid} not found")
      : Result<StoreResponse>.Success(new StoreResponse(
        store.Id,
        store.Cnpj.Value,
        store.Name,
        store.Email.Value,
        store.IsActive
      ));
  }
}