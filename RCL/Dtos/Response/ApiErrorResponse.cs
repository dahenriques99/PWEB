namespace RCL.Dtos.Response;

public sealed class ApiErrorResponse
{
    public List<string> Errors { get; set; } = new();
}