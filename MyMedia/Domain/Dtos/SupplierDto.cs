namespace MyMedia.Domain.Dtos;

public class SupplierDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string NIF { get; set; } = string.Empty;
    public byte[] LogoData { get; set; } = [];
}