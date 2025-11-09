using KluskaStore.Domain.Entities;
using KluskaStore.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KluskaStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(AppDbContext context) : ControllerBase {
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Product>>> GetAll() {
    var products = await context.Products.ToListAsync();
    return Ok(products);
  }
}