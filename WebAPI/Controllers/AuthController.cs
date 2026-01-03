using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyMedia.Infrastructure;
using MyMedia.Infrastructure.Entities.enums;
using RCL.Dtos.Request;
using RCL.Dtos.Response;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    IConfiguration configuration,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager)
    : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto req)
    {
        var user = await userManager.FindByEmailAsync(req.Email);
        if (user is null) return Unauthorized();

        if (user.Status == UserStatus.Pending)
            return BadRequest(new { errors = new[] { "Account is awaiting approval." } });

        var ok = await signInManager.CheckPasswordSignInAsync(user, req.Password, lockoutOnFailure: false);
        if (!ok.Succeeded) return Unauthorized();

        var roles = await userManager.GetRolesAsync(user);

        var jwt = configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new(ClaimTypes.Name, user.UserName ?? user.Email ?? ""),
            new("ClientType", user.ClientType.ToString()),
            new("Status", user.Status.ToString())
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        if (!int.TryParse(jwt["ExpiresMinutes"], out var expMinutes))
            expMinutes = 60;

        var expires = DateTime.UtcNow.AddMinutes(expMinutes);
        
        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return Ok(new AuthResponseDto
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAtUtc = expires
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto requestDto)
    {
        var existingByEmail = await userManager.FindByEmailAsync(requestDto.Email);
        if (existingByEmail != null)
            return BadRequest(new { errors = new[] { "Email already in use." } });
        
        var user = new ApplicationUser
        {
            UserName = requestDto.Email,
            Email = requestDto.Email,
            Surname = requestDto.Surname,
            Name = requestDto.Name,
            Nif = requestDto.Nif,

            ClientType = UserType.Client,
            Status = UserStatus.Pending
        };

        var create = await userManager.CreateAsync(user, requestDto.Password);
        if (!create.Succeeded)
            return BadRequest(new { errors = create.Errors.Select(e => e.Description) });

        var addRole = await userManager.AddToRoleAsync(user, nameof(UserRoles.Client));
        if (!addRole.Succeeded)
            return BadRequest(new { errors = addRole.Errors.Select(e => e.Description) });

        return CreatedAtAction(nameof(Register),
            new RegisterResponse { UserId = user.Id });
    }
    
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me() => Ok(new { ok = true });

    [HttpGet("debug/users")]
    public IEnumerable<object> DebugUsers([FromServices] UserManager<ApplicationUser> um)
        => um.Users.Select(u => new { u.Id, u.Email });

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