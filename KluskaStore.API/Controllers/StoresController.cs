using KluskaStore.Application.Abstractions;
using KluskaStore.Application.Features.Stores;
using KluskaStore.Application.Features.Stores.CreateStore;
using KluskaStore.Application.Features.Stores.GetStoreByCnpj;
using KluskaStore.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KluskaStore.API.Controllers;

// TODO: implement UPDATE and DELETE
[ApiController]
[Route("api/[controller]")]
public class StoresController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStoreWithAddressCommand request, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var resultResult = await mediator.Send(request, ct);
        return resultResult.IsFailure
            ? BadRequest(resultResult.Errors)
            : CreatedAtAction(
                nameof(GetById),
                new { id = resultResult.Value.Id },
                resultResult.Value
            );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var request = new GuidQuery<StoreResponse>(id);
        var storeResult = await mediator.Send(request, ct);
        return storeResult.IsFailure ? NotFound(storeResult.Errors) : Ok(storeResult.Value);
    }

    [HttpGet("{cnpj}")]
    public async Task<IActionResult> GetByCnpj(string cnpj, CancellationToken ct)
    {
        var cnpjResult = Cnpj.Create(cnpj, skipVerifierDigitsValidation: true);
        if (cnpjResult.IsFailure) return BadRequest(cnpjResult.Errors);

        var request = new CnpjQuery(cnpjResult.Value);
        var storeResult = await mediator.Send(request, ct);
        return storeResult.IsFailure ? NotFound(storeResult.Errors) : Ok(storeResult.Value);
    }
}