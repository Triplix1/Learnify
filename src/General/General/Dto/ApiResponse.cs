namespace General.Dto;

/// <summary>
/// Response which will be returned from each endpoint
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Gets or sets value for Data
    /// </summary>
    public T Data { get; set; }
    
    /// <summary>
    /// Gets or sets value for Error
    /// </summary>
    public string? Error { get; set; }
}