namespace Learnify.Core.Dto;

public class ApiResponse
{
    public object Data { get; set; }
    public string ErrorMessage { get; set; }
    public string ErrorStackTrace { get; set; }
    public bool IsSuccess { get; set; }

    protected ApiResponse(object data)
    {
        Data = data;
    }

    protected ApiResponse(Exception error) : this(error.Message, error.StackTrace)
    {
    }

    protected ApiResponse(string errorMessage, string stackTrace)
    {
        ErrorMessage = errorMessage;
        ErrorStackTrace = stackTrace;
        IsSuccess = false;
    }

    public static ApiResponse Success(object value)
    {
        return new ApiResponse(value);
    }

    public static ApiResponse Failure(Exception error)
    {
        return new ApiResponse(error);
    }

    public static ApiResponse Failure(string errorMessage, string stackTrace)
    {
        return new ApiResponse(errorMessage, stackTrace);
    }
}