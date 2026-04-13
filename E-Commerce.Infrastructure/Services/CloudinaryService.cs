using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using E_Commerce.Application.Common;
using E_Commerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace E_Commerce.Infrastructure.Services;

public class CloudinaryService : IFileService
{
    private readonly CloudinarySettings _cloudinarySettings;
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IOptionsSnapshot<CloudinarySettings> cloudinarySettings)
    {
        _cloudinarySettings = cloudinarySettings.Value;
        _cloudinary = new Cloudinary(new Account(_cloudinarySettings.CloudName, _cloudinarySettings.ApiKey, _cloudinarySettings.ApiSecret));
    }

    public async Task<bool> DeleteImageAsync(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            return false;

        var publicId = Path.GetFileNameWithoutExtension(imageUrl);
        if (string.IsNullOrWhiteSpace(publicId))
            return false;

        var deletionParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deletionParams);

        return result.Result == "ok";
    }

    public async Task<string?> UploadImageAsync(IFormFile file)
    {
        if (file is null || file.Length == 0) return null;

        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Height(500).Width(500).Crop("fill")
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null)
        {
            return null;
        }

        return uploadResult.SecureUrl.AbsoluteUri;
    }
}
