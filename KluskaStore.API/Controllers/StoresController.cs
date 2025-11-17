using KluskaStore.Application.Features.Stores.CreateStore;
using KluskaStore.Application.Features.Stores.GetStoreById;
using KluskaStore.Application.Interfaces.Stores;
using Microsoft.AspNetCore.Mvc;

namespace KluskaStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoresController(IStoreService service) : ControllerBase {
  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateStoreCommand request, CancellationToken ct) {
    if (!ModelState.IsValid) return BadRequest(ModelState);
    var resultResult = await service.CreateStoreAsync(request, ct);
    return resultResult.IsFailure
      ? BadRequest(resultResult.Errors)
      : CreatedAtAction(nameof(GetById), new { id = resultResult.Value.Id }, resultResult.Value);
  }

  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetById(Guid id, CancellationToken ct) {
    var request = new GetStoreByIdCommand(id);
    var storeResult = await service.GetStoreByIdAsync(request, ct);
    return storeResult.IsFailure ? NotFound(storeResult.Errors) : Ok(storeResult.Value);
  }
}