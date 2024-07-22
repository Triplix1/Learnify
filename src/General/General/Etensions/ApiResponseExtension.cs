using General.Dto;

namespace General.Etensions;

/// <summary>
/// Extension class for Each object
/// </summary>
public static class ApiResponseExtension
{
    /// <summary>
    /// Converts data to ApiResponse
    /// </summary>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ApiResponse<T> ToApiResponse<T>(this T obj)
    {
        return new ApiResponse<T>()
        {
            Data = obj
        };
    }
}