using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyMedia.Infrastructure;
using RCL.Dtos;

namespace WebAPI.Controllers;

public class AuthController(
    IConfiguration configuration,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager)
    : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            //ModelStats has base model (dto) validation
            return BadRequest(ModelState);
        }
        
        var user = await userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials");
        var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!result.Succeeded)
            throw new UnauthorizedAccessException("Invalid credentials");
        
        var token = GenerateJwtToken(user);
        return Ok(new AuthResponseDto
        {
            AccessToken = token,
            TokenType = "Bearer",
            ExpiresIn = 3600,
            Email = user.Email ?? string.Empty
        });
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var jwtKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        // Configure signature algorithm
        var creeds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); 


        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
        };

        // Add roles
        var roles = userManager.GetRolesAsync(user).Result;
        //foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: creeds
        );

        // Convert token to string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}