using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace General.CommonServiceContracts;

public interface IPhotoService
{
    Task<ImageUploadResult> AddPhotoAsync(IFormFile file, int height, int width);
    Task<DeletionResult> DeletePhotoAsync(string publicId);
}