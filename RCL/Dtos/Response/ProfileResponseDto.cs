namespace RCL.Dtos.Response;

public sealed class ProfileDto
{
    public string UserId { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Nif { get; set; }
    public string Status { get; set; } = default!;
}