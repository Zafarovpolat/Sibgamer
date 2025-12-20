using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Services;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UploadController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly ILogger<UploadController> _logger;

    public UploadController(IFileService fileService, ILogger<UploadController> logger)
    {
        _fileService = fileService;
        _logger = logger;
    }

    [HttpPost("slider")]
    public async Task<IActionResult> UploadSliderImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "Файл не выбран" });

        if (!_fileService.ValidateImageFile(file))
            return BadRequest(new { message = "Недопустимый формат файла. Разрешены: jpg, jpeg, png, gif, webp" });

        if (!_fileService.ValidateFileSize(file, 5))
            return BadRequest(new { message = "Размер файла не должен превышать 5 МБ" });

        try
        {
            var imageUrl = await _fileService.SaveFileAsync(file, "slider");
            return Ok(new { url = imageUrl });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading slider image");
            return StatusCode(500, new { message = "Ошибка при загрузке файла" });
        }
    }

    [HttpPost("maps")]
    public async Task<IActionResult> UploadMapImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "Файл не выбран" });

        if (!_fileService.ValidateImageFile(file))
            return BadRequest(new { message = "Недопустимый формат файла. Разрешены: jpg, jpeg, png" });

        if (!_fileService.ValidateFileSize(file, 2))
            return BadRequest(new { message = "Размер файла не должен превышать 2 МБ" });

        try
        {
            var imageUrl = await _fileService.SaveFileAsync(file, "maps");
            return Ok(new { url = imageUrl });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading map image");
            return StatusCode(500, new { message = "Ошибка при загрузке файла" });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("news")]
    public async Task<IActionResult> UploadNewsImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "Файл не выбран" });

        if (!_fileService.ValidateImageFile(file))
            return BadRequest(new { message = "Недопустимый формат файла. Разрешены: jpg, jpeg, png, gif, webp" });

        if (!_fileService.ValidateFileSize(file, 10))
            return BadRequest(new { message = "Размер файла не должен превышать 10 МБ" });

        try
        {
            var imageUrl = await _fileService.SaveFileAsync(file, "news");
            return Ok(new { url = imageUrl });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading news image");
            return StatusCode(500, new { message = "Ошибка при загрузке файла" });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("events")]
    public async Task<IActionResult> UploadEventImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "Файл не выбран" });

        if (!_fileService.ValidateImageFile(file))
            return BadRequest(new { message = "Недопустимый формат файла. Разрешены: jpg, jpeg, png, gif, webp" });

        if (!_fileService.ValidateFileSize(file, 10))
            return BadRequest(new { message = "Размер файла не должен превышать 10 МБ" });

        try
        {
            var imageUrl = await _fileService.SaveFileAsync(file, "events");
            return Ok(new { url = imageUrl });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading event image");
            return StatusCode(500, new { message = "Ошибка при загрузке файла" });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("custompages")]
    public async Task<IActionResult> UploadCustomPageImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "Файл не выбран" });

        if (!_fileService.ValidateImageFile(file))
            return BadRequest(new { message = "Недопустимый формат файла. Разрешены: jpg, jpeg, png, gif, webp" });

        if (!_fileService.ValidateFileSize(file, 10))
            return BadRequest(new { message = "Размер файла не должен превышать 10 МБ" });

        try
        {
            var imageUrl = await _fileService.SaveFileAsync(file, "custompages");
            return Ok(new { url = imageUrl });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading custom page image");
            return StatusCode(500, new { message = "Ошибка при загрузке файла" });
        }
    }

    [AllowAnonymous]
    [HttpPost("avatar")]
    public async Task<IActionResult> UploadAvatar(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "Файл не выбран" });

        if (!_fileService.ValidateImageFile(file))
            return BadRequest(new { message = "Недопустимый формат файла. Разрешены: jpg, jpeg, png, gif, webp" });

        if (!_fileService.ValidateFileSize(file, 2))
            return BadRequest(new { message = "Размер файла не должен превышать 2 МБ" });

        try
        {
            var imageUrl = await _fileService.SaveFileAsync(file, "avatars");
            return Ok(new { url = imageUrl });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading avatar");
            return StatusCode(500, new { message = "Ошибка при загрузке файла" });
        }
    }
}

