namespace Learnify.Core.Dto;

/// <summary>
/// Response which will be returned from each endpoint
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResponse<T>: ApiResponse
{
    /// <summary>
    /// Initializes a new instance of <see cref="ApiResponse{T}"/> 
    /// </summary>
    /// <param name="data">Data</param>
    protected ApiResponse(T data)
    {
        Data = data;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ApiResponse{T}"/> 
    /// </summary>
    /// <param name="error">Error</param>
    protected ApiResponse(Exception error): base(error)
    {
    }
    
    /// <summary>
    /// Gets or sets value for Data
    /// </summary>
    public T? Data { get; set; }
    
    /// <summary>
    /// Creates a new success instance
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static ApiResponse<T> Success(T value)
    {
        return new ApiResponse<T>(value);
    }
    /// <summary>
    /// Creates a new error instance
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public new static ApiResponse<T> Failure(Exception error)
    {
        return new ApiResponse<T>(error);
    }
}

/// <summary>
/// Base empty api response
/// </summary>
public class ApiResponse
{
    /// <summary>
    /// Initializes a new instance of <see cref="ApiResponse"/> 
    /// </summary>
    protected ApiResponse()
    {
        IsSuccess = true;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ApiResponse"/>
    /// </summary>
    /// <param name="error">Error</param>
    protected ApiResponse(Exception error)
    {
        ErrorMessage = error.Message;
        ErrorStackTrace = error.StackTrace;
        IsSuccess = false;
    }
    
    /// <summary>
    /// Gets or sets value for Error
    /// </summary>
    public string ErrorMessage { get; set; }
    public string ErrorStackTrace { get; set; }
    
    /// <summary>
    /// Gets or sets value for IsSuccess
    /// </summary>
    public bool IsSuccess { get; set; }
    
    /// <summary>
    /// Creates a new success instance
    /// </summary>
    /// <returns></returns>
    public static ApiResponse Success()
    {
        return new ApiResponse();
    }
    /// <summary>
    /// Creates a new error instance
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static ApiResponse Failure(Exception error)
    {
        return new ApiResponse(error);
    }
}