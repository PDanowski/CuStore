using CuStore.Core.Abstractions;
using CuStore.Api.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CuStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository productRepository) : ControllerBase
{
    [HttpGet]
    public IActionResult List([FromQuery] int? categoryId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var products = productRepository
            .GetProductsByCategory(pageSize, pageNumber, categoryId)
            .Select(p => new ProductSummaryResponse(
                p.Id,
                p.Name,
                p.Description,
                p.Price))
            .ToList();

        var total = productRepository.GetProductsCount(categoryId);

        return Ok(new ProductListResponse(products, total, pageNumber, pageSize));
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var product = productRepository.GetProductById(id);
        return product is null ? NotFound() : Ok(product);
    }
}
