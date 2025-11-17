using KluskaStore.Application.Features.Stores.CreateStore;
using KluskaStore.Application.Features.Stores.GetStoreById;
using KluskaStore.Application.Interfaces;
using KluskaStore.Application.Interfaces.Stores;
using KluskaStore.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KluskaStore.Application;

public static class DependencyInjection {
  public static IServiceCollection AddApplication(this IServiceCollection services) {
    services.AddScoped<ICreateStoreHandler, CreateStoreHandler>();
    services.AddScoped<IGetStoreByIdHandler, GetStoreByIdHandler>();
    services.AddScoped<IStoreService, StoreService>();
    return services;
  }
}