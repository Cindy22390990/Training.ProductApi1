namespace Training.ProductApi1.Models.DTOs;

public class ApiResponse<T> : ApiResponseBase
{

    public T? Data { get; set; }


    public static ApiResponse<T> SuccessResult(
        T data,
        string message = "操作成功",
        int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            Success = true,
            StatusCode = statusCode,
            Message = message,
            Data = data
        };
    }


    public static ApiResponse<T> FailResult(
        string message,
        int statusCode = 400)
    {
        return new ApiResponse<T>
        {
            Success = false,
            StatusCode = statusCode,
            Message = message,
            Data = default
        };
    }
}