using KluskaStore.Application.DTOs;
using KluskaStore.Application.DTOs.Store;
using KluskaStore.Application.Exceptions;
using KluskaStore.Application.Interfaces;
using KluskaStore.Domain.Repositories;

namespace KluskaStore.Application.UseCases.Store;

public class GetStoreByIdUseCase(IUnitOfWork uow) : UseCase<GuidRequest, StoreResponse>(uow), IGetStoreByIdUseCase {
  public override async Task<StoreResponse> ExecuteAsync(GuidRequest request) {
    var store = await UoW.Stores.GetByIdAsync(request.Guid);

    if (store is null) throw NotFoundException.Store(request.Guid);
    
    return new StoreResponse(store.Id, store.Cnpj.Value ,store.Name, store.Email.Value, store.IsActive);
  }
}