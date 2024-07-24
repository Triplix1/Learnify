namespace General.Dto;

/// <summary>
/// Response which will be returned from each endpoint
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResponse<T>: ApiResponse
{
    protected ApiResponse(T data)
    {
        Data = data;
    }

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

public class ApiResponse
{
    protected ApiResponse()
    {
        IsSuccess = true;
    }

    protected ApiResponse(Exception error)
    {
        Error = error;
        IsSuccess = false;
    }
    
    /// <summary>
    /// Gets or sets value for Error
    /// </summary>
    public Exception Error { get; set; }
    
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