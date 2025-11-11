using KluskaStore.Application.Interfaces;
using KluskaStore.Application.Services;
using KluskaStore.Application.UseCases.Store;
using Microsoft.Extensions.DependencyInjection;

namespace KluskaStore.Application;

public static class DependencyInjection {
  public static IServiceCollection AddApplication(this IServiceCollection services) {
    services.AddScoped<ICreateStoreUseCase, CreateStoreUseCase>();
    services.AddScoped<IGetStoreByIdUseCase, GetStoreByIdUseCase>();
    services.AddScoped<IStoreService, StoreService>();
    return services;
  }
}