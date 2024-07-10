using General.Dto;

namespace General.Etensions;

public static class ApiResponseExtension
{
    public static ApiResponse<T> ToApiResponse<T>(this T obj)
    {
        return new ApiResponse<T>()
        {
            Data = obj
        };
    }
}