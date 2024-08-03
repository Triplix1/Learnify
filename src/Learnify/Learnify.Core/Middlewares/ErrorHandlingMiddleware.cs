using System.Net;
using System.Text;
using Learnify.Core.Dto;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Learnify.Core.Middlewares;

/// <summary>
/// Middleware for handling unexpected errors
/// </summary>
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of <see cref="ErrorHandlingMiddleware"/>
    /// </summary>
    /// <param name="next"></param>
    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invokes middleware
    /// </summary>
    /// <param name="context"><see cref="HttpContext"/></param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Call the next middleware in the pipeline
            await _next(context);
        }
        catch (Exception ex)
        {
            // Handle the exception and return a formatted ApiResponse
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Set the response status code and content type
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        // Create the ApiResponse with the error message
        var response = ApiResponse.Failure(exception);

        // Serialize the response to JSON
        var jsonResponse = JsonConvert.SerializeObject(response);

        // Write the JSON response to the HTTP response
        return context.Response.WriteAsync(jsonResponse);
    }
}