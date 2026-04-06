using CuStore.Core.Abstractions;
using CuStore.WebApi.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CuStore.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShippingMethodsController(IShippingMethodRepository shippingMethodRepository) : ControllerBase
{
    [HttpGet]
    public IActionResult List()
    {
        var methods = shippingMethodRepository
            .GetShippingMethods()
            .Select(sm => new ShippingMethodResponse(sm.Id, sm.Name, sm.Price, sm.MaximumPcs))
            .ToList();

        return Ok(methods);
    }
}
