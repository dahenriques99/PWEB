using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RCL.Dtos.Request;
using WebAPI.Repositories;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderRepository orderRepository) : ControllerBase
{
    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] CheckoutRequestDto req)
    {
        if (req?.Items is null || req.Items.Count == 0)
            return BadRequest(new { errors = new[] { "Cart is empty." } });

        var userId =
            User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        var result = await orderRepository.CreateOrderFromCart(userId, req.Items);

        if (!result.Success)
            return BadRequest(new { errors = result.Errors });

        return Ok(result.Response);
    }
}