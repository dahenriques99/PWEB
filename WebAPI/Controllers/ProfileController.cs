using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMedia.Infrastructure;
using MyMedia.Infrastructure.Entities.enums;
using RCL.Dtos.Response;
using WebAPI.Repositories;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController(IUserRepository userRepository, UserManager<ApplicationUser> userManager)
    : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<ProfileDto>> GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

        var user = await userManager.FindByIdAsync(userId);
        if (user is null) return NotFound();

        return Ok(new ProfileDto
        {
            UserId = user.Id,
            Email = user.Email ?? "",
            Name = user.Name,
            Surname = user.Surname,
            Nif = user.Nif,
            Status = user.Status.ToString()
        });
    }

    [Authorize]
    [HttpGet("orders")]
    public async Task<ActionResult<IEnumerable<OrderDetailsDto>>> GetUserOrders()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

        var orders = await userRepository.GetUserOrders(userId);
        return Ok(orders);
    }
    
    [Authorize(Roles = nameof(UserRoles.Supplier))]
    [HttpGet("products")]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetUserProducts()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

        var list = await userRepository.GetUserProducts(userId);
        return Ok(list);
    }
}