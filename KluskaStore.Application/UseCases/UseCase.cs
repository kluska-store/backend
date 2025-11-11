using KluskaStore.Application.Interfaces;
using KluskaStore.Domain.Repositories;

namespace KluskaStore.Application.UseCases;

public abstract class UseCase<TReq, TResp>(IUnitOfWork uow) : IUseCase<TReq, TResp> where TReq : IDto where TResp : IDto {
  protected readonly IUnitOfWork UoW = uow;
  public abstract Task<TResp> ExecuteAsync(TReq request);
}