using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace General.CommonServiceContracts;

/// <summary>
/// Photo Service
/// </summary>
[Obsolete]
public interface IPhotoService
{
    /// <summary>
    /// Saves photo
    /// </summary>
    /// <param name="file">File to save</param>
    /// <param name="height">height in pixels</param>
    /// <param name="width">width in pixels</param>
    /// <returns></returns>
    Task<ImageUploadResult> AddPhotoAsync(IFormFile file, int height, int width);
    
    /// <summary>
    /// Deletes photo
    /// </summary>
    /// <param name="publicId">Photo public id</param>
    /// <returns></returns>
    Task<DeletionResult> DeletePhotoAsync(string publicId);
}