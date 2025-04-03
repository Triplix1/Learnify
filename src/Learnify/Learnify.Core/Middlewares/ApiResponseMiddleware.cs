using System.Text.Json;
using Learnify.Core.Attributes;
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
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<SkipApiResponseAttribute>() != null)
        {
            await _next(context);
            return; // Skip ApiResponse wrapping for this endpoint
        }
        
        var originalResponseBodyStream = context.Response.Body;

        using (var responseBodyStream = new MemoryStream())
        {
            context.Response.Body = responseBodyStream;

            try
            {
                await _next(context);

                context.Response.Body = originalResponseBodyStream;

                if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
                {
                    var responseBody = await ReadStreamAsync(responseBodyStream);

                    if (!responseBody.StartsWith("{") && !responseBody.StartsWith("["))
                    {
                        responseBody = "{" + responseBody + "}";
                    }
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