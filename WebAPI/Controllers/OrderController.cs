using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyMedia.Infrastructure.Entities.enums;
using RCL.Dtos.Request;
using RCL.Dtos.Response;
using WebAPI.Repositories;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderRepository orderRepository) : ControllerBase
{
    [Authorize(Roles = nameof(UserRoles.Client))]
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

        var result = await orderRepository.CreateOrderFromCart(userId, req);

        if (!result.Success)
            return BadRequest(new { errors = result.Errors });

        return Ok(result.Response);
    }
    
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDetailsDto>> GetOrder(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

        var dto = await orderRepository.GetOrderDetails(id, userId);
        if (dto is null) return NotFound();

        return Ok(dto);
    }
}