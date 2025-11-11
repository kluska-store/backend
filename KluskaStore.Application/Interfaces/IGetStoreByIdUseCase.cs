using KluskaStore.Application.DTOs;
using KluskaStore.Application.DTOs.Store;

namespace KluskaStore.Application.Interfaces;

public interface IGetStoreByIdUseCase : IUseCase<GuidRequest, StoreResponse> { }