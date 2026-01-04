using Microsoft.AspNetCore.Mvc;
using WebAPI.Repositories;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeliveryMethodsController(IDeliveryMethodsRepository deliveryMethodsRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetDeliveryMethods()
    {
        var products = await deliveryMethodsRepository.GetDeliveryMethods();
        return Ok(products);
    }
}