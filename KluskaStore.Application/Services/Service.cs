using KluskaStore.Application.Interfaces;
using KluskaStore.Domain.Repositories;

namespace KluskaStore.Application.Services;

public class Service : IService {
  protected readonly IUnitOfWork UoW;

  protected Service(IUnitOfWork uow) {
    UoW = uow;
  }
}