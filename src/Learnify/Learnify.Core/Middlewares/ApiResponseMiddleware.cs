using System.Text.Json;
using Learnify.Core.Dto;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Learnify.Core.Middlewares;

/// <summary>
/// Middleware for handling unexpected errors
/// </summary>
public class ApiResponseMiddleware
{
    private readonly RequestDelegate _next;

    public ApiResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalResponseBodyStream = context.Response.Body;

        using (var responseBodyStream = new MemoryStream())
        {
            context.Response.Body = responseBodyStream;

            try
            {
                await _next(context);

                context.Response.Body = originalResponseBodyStream;

                // Skip wrapping if response is a file
                if (IsFileResponse(context.Response))
                {
                    await responseBodyStream.CopyToAsync(originalResponseBodyStream);
                    return;
                }

                if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
                {
                    var responseBody = await ReadStreamAsync(responseBodyStream);
                    var data = string.IsNullOrWhiteSpace(responseBody)
                        ? null
                        : JsonSerializer.Deserialize<object>(responseBody);

                    var apiResponse = ApiResponse.Success(data);

                    await WriteResponseAsync(context, apiResponse, StatusCodes.Status200OK);
                }
                else
                {
                    var responseBody = await ReadStreamAsync(responseBodyStream);

                    var apiResponse = ApiResponse.Failure(responseBody, stackTrace: string.Empty);

                    await WriteResponseAsync(context, apiResponse, StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex)
            {
                context.Response.Body = originalResponseBodyStream;

                var errorResponse = ApiResponse.Failure(ex.Message, ex.StackTrace);
                await WriteResponseAsync(context, errorResponse, StatusCodes.Status500InternalServerError);
            }
            finally
            {
                context.Response.Body = originalResponseBodyStream;
            }
        }
    }

    private bool IsFileResponse(HttpResponse response)
    {
        // Check for common file content types
        return response.ContentType?.StartsWith("application/octet-stream") == true ||
               response.ContentType?.StartsWith("application/pdf") == true ||
               response.ContentType?.StartsWith("image/") == true ||
               response.ContentType?.StartsWith("text/csv") == true;
    }

    private async Task<string> ReadStreamAsync(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        using (var reader = new StreamReader(stream))
        {
            return await reader.ReadToEndAsync();
        }
    }

    private async Task WriteResponseAsync(HttpContext context, object apiResponse, int statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        var responseJson = JsonSerializer.Serialize(apiResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });
        await context.Response.WriteAsync(responseJson);
    }
}

public static class ApiResponseMiddlewareExtensions
{
    public static IApplicationBuilder UseApiResponseMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ApiResponseMiddleware>();
    }
}