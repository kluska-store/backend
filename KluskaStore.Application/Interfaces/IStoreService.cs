using KluskaStore.Application.DTOs;
using KluskaStore.Application.DTOs.Store;

namespace KluskaStore.Application.Interfaces;

public interface IStoreService : IService {
  Task<StoreResponse> GetStoreByIdAsync(GuidRequest request);
  Task<StoreResponse> CreateStoreAsync(CreateStoreRequest request);
}