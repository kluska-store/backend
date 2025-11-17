using KluskaStore.Application.Features.Stores;
using KluskaStore.Application.Features.Stores.CreateStore;
using KluskaStore.Application.Features.Stores.GetStoreById;
using KluskaStore.Domain.Shared;

namespace KluskaStore.Application.Interfaces.Stores;

public interface IStoreService : IService {
  Task<Result<StoreResponse>> GetStoreByIdAsync(GetStoreByIdCommand request, CancellationToken cancellationToken);
  Task<Result<StoreResponse>> CreateStoreAsync(CreateStoreCommand request, CancellationToken cancellationToken);
}