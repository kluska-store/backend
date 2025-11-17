using KluskaStore.Application.Abstractions;
using KluskaStore.Application.Features.Stores;
using KluskaStore.Application.Features.Stores.CreateStore;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KluskaStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoresController(IMediator mediator) : ControllerBase {
  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateStoreCommand request, CancellationToken ct) {
    if (!ModelState.IsValid) return BadRequest(ModelState);
    var resultResult = await mediator.Send(request, ct);
    return resultResult.IsFailure
      ? BadRequest(resultResult.Errors)
      : CreatedAtAction(nameof(GetById), new { id = resultResult.Value.Id }, resultResult.Value);
  }

  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetById(Guid id, CancellationToken ct) {
    var request = new GuidQuery<StoreResponse>(id);
    var storeResult = await mediator.Send(request, ct);
    return storeResult.IsFailure ? NotFound(storeResult.Errors) : Ok(storeResult.Value);
  }
}