using System.Net;
using System.Text.Json;
using GlobalExceptionHandlingDemo.Exceptions;
using KeyNotFoundException = GlobalExceptionHandlingDemo.Exceptions.KeyNotFoundException;
using NotImplementedException = GlobalExceptionHandlingDemo.Exceptions.NotImplementedException;
using UnauthorizedAccessException = GlobalExceptionHandlingDemo.Exceptions.UnauthorizedAccessException;

namespace GlobalExceptionHandlingDemo.Utility;

public class GlobalErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode;
        var stackTrace = exception.StackTrace;
        var message = exception.Message;
        switch (exception)
        {
            case BadRequestException:
                statusCode = HttpStatusCode.BadRequest;
                break;
            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                break;
            case NotImplementedException:
                statusCode = HttpStatusCode.NotImplemented;
                break;
            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                break;
            case KeyNotFoundException:
                statusCode = HttpStatusCode.Unauthorized;
                break;
            default:
                message = exception.Message;
                statusCode = HttpStatusCode.InternalServerError;
                break;
        }

        var exceptionResult = JsonSerializer.Serialize(new { error = message, stackTrace });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(exceptionResult);
    }
}