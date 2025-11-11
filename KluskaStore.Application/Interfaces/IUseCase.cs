namespace KluskaStore.Application.Interfaces;

public interface IUseCase<in TReq, TResp> where TReq : IDto where TResp : IDto {
  Task<TResp> ExecuteAsync(TReq request);
}