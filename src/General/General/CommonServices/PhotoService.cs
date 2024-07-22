using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using General.CommonServiceContracts;
using General.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace General.CommonServices;

/// <inheritdoc />
public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;

    /// <summary>
    /// Initializes a new instance of PhotoService
    /// </summary>
    /// <param name="config"><see cref="IOptions{TOptions}"/></param>
    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var acc = new Account
        (
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(acc);
    }

    /// <inheritdoc />
    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file, int height, int width)
    {
        var uploadResult = new ImageUploadResult();

        if (file.Length <= 0) return uploadResult;

        await using var stream = file.OpenReadStream();
        
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Height(height).Width(width).Crop("fill").Gravity("face"),
            Folder = "Relaxinema"
        };
        uploadResult = await _cloudinary.UploadAsync(uploadParams);

        return uploadResult;
    }

    /// <inheritdoc />
    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        var deletionParams = new DeletionParams(publicId);

        return await _cloudinary.DestroyAsync(deletionParams);   
    }
}