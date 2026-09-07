using System.Net;
using Training.ProductApi1.Models.DTOs;


namespace Training.ProductApi1.Middleware;


public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionMiddleware> _logger;


    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }


    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {

            _logger.LogError(
                ex,
                "系統發生錯誤");


            var statusCode = HttpStatusCode.InternalServerError;


            var message = "系統發生錯誤";


            if (ex.Message.Contains("不存在"))
            {
                statusCode = HttpStatusCode.NotFound;

                message = ex.Message;
            }


            else if (ex.Message.Contains("已綁定"))
            {
                statusCode = HttpStatusCode.Conflict;

                message = ex.Message;
            }


            else if (ex is ArgumentException)
            {
                statusCode = HttpStatusCode.BadRequest;

                message = ex.Message;
            }



            context.Response.StatusCode =
                (int)statusCode;


            context.Response.ContentType =
                "application/json";



            var response =
                ApiResponse<string>
                .FailResult(
                    message,
                    (int)statusCode);



            await context.Response
                .WriteAsJsonAsync(response);
        }
    }
}