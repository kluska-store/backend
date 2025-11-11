using KluskaStore.Application.DTOs;
using KluskaStore.Application.DTOs.Store;
using KluskaStore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KluskaStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoresController : ControllerBase {
  private readonly IStoreService _service;

  public StoresController(IStoreService service) {
    _service = service;
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateStoreRequest request) {
    if (!ModelState.IsValid) return BadRequest(ModelState);
    
    var store = await _service.CreateStoreAsync(request);

    return CreatedAtAction(nameof(GetById), new { id = store.Id }, store);
  }

  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetById(Guid id) {
    var store = await _service.GetStoreByIdAsync(new GuidRequest(id));
    return Ok(store);
  }
}