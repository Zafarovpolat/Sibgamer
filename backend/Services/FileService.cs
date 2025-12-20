using Microsoft.AspNetCore.StaticFiles;

namespace backend.Services;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file, string folder);
    Task DeleteFileAsync(string filePath);
    string GetMimeType(string fileName);
    bool ValidateImageFile(IFormFile file);
    bool ValidateFileSize(IFormFile file, long maxSizeInMb);
}

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<FileService> _logger;
    private readonly FileExtensionContentTypeProvider _contentTypeProvider;

    private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private static readonly string[] AllowedImageMimeTypes = { "image/jpeg", "image/png", "image/gif", "image/webp" };

    public FileService(IWebHostEnvironment environment, ILogger<FileService> logger)
    {
        _environment = environment;
        _logger = logger;
        _contentTypeProvider = new FileExtensionContentTypeProvider();
    }

    public async Task<string> SaveFileAsync(IFormFile file, string folder)
    {
        try
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", folder);
            Directory.CreateDirectory(uploadsFolder);

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{folder}/{fileName}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving file to folder {Folder}", folder);
            throw;
        }
    }

    public async Task DeleteFileAsync(string relativeFilePath)
    {
        if (string.IsNullOrWhiteSpace(relativeFilePath))
            return;

        try
        {
            relativeFilePath = relativeFilePath.TrimStart('/');

            var fullPath = Path.Combine(_environment.WebRootPath, relativeFilePath);

            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath));
                _logger.LogInformation("Deleted file: {FilePath}", fullPath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {FilePath}", relativeFilePath);
        }
    }

    public string GetMimeType(string fileName)
    {
        if (_contentTypeProvider.TryGetContentType(fileName, out var contentType))
        {
            return contentType;
        }

        return "application/octet-stream";
    }

    public bool ValidateImageFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedImageExtensions.Contains(extension))
            return false;

        if (!AllowedImageMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
            return false;

        try
        {
            using var stream = file.OpenReadStream();
            var buffer = new byte[8];
            stream.Read(buffer, 0, buffer.Length);

            if (IsJpeg(buffer) || IsPng(buffer) || IsGif(buffer) || IsWebp(buffer))
            {
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    public bool ValidateFileSize(IFormFile file, long maxSizeInMb)
    {
        return file != null && file.Length > 0 && file.Length <= maxSizeInMb * 1024 * 1024;
    }

    private static bool IsJpeg(byte[] bytes)
    {
        return bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xD8;
    }

    private static bool IsPng(byte[] bytes)
    {
        return bytes.Length >= 8 
            && bytes[0] == 0x89 && bytes[1] == 0x50 
            && bytes[2] == 0x4E && bytes[3] == 0x47;
    }

    private static bool IsGif(byte[] bytes)
    {
        return bytes.Length >= 3 
            && bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46;
    }

    private static bool IsWebp(byte[] bytes)
    {
        return bytes.Length >= 12 
            && bytes[0] == 0x52 && bytes[1] == 0x49 
            && bytes[2] == 0x46 && bytes[3] == 0x46
            && bytes[8] == 0x57 && bytes[9] == 0x45 
            && bytes[10] == 0x42 && bytes[11] == 0x50;
    }
}
