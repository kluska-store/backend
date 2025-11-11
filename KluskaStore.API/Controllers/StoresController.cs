using KluskaStore.Application.DTOs.Store;
using KluskaStore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KluskaStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoresController : ControllerBase {
  private readonly ICreateStoreUseCase _createStoreUseCase;

  public StoresController(ICreateStoreUseCase createStoreUseCase) {
    _createStoreUseCase = createStoreUseCase;
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateStoreRequest request) {
    if (!ModelState.IsValid) return BadRequest(ModelState);
    
    var store = await _createStoreUseCase.ExecuteAsync(request);

    return CreatedAtAction(nameof(GetById), new { id = store.Id }, store);
  }

  [HttpGet]
  public async Task<IActionResult> GetById(Guid id) {
    // TODO: implement method
    return Ok();
  }
}