namespace Training.ProductApi1.Models.DTOs;

public class ApiResponseBase
{
    public bool Success { get; set; }

    public int StatusCode { get; set; }

    public string Message { get; set; } = string.Empty;
}