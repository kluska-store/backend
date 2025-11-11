using KluskaStore.Application.DTOs;
using KluskaStore.Application.DTOs.Store;
using KluskaStore.Application.Interfaces;
using KluskaStore.Domain.Repositories;

namespace KluskaStore.Application.Services;

public class StoreService : Service, IStoreService {
  private readonly ICreateStoreUseCase _createStoreUseCase;
  private readonly IGetStoreByIdUseCase _getStoreByIdUseCase;

  public StoreService(
    IUnitOfWork uow,
    ICreateStoreUseCase createStoreUseCase,
    IGetStoreByIdUseCase getStoreByIdUseCase
  ) : base(uow) {
    _createStoreUseCase = createStoreUseCase;
    _getStoreByIdUseCase = getStoreByIdUseCase;
  }

  public async Task<StoreResponse> GetStoreByIdAsync(GuidRequest request) =>
    await _getStoreByIdUseCase.ExecuteAsync(request);

  public async Task<StoreResponse> CreateStoreAsync(CreateStoreRequest request) =>
    await _createStoreUseCase.ExecuteAsync(request);
}