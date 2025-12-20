using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.Services;
using backend.Utils;
using backend.Data.DTOs;

namespace backend.Controllers;

[ApiController]
[Route("api/admin/slider")]
[Authorize(Roles = "Admin")]
public class AdminSliderController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IFileService _fileService;

    public AdminSliderController(ApplicationDbContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SliderImage>>> GetSliderImages()
    {
        var images = await _context.SliderImages
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        Console.WriteLine($"Returning {images.Count} slider images");
        foreach (var img in images)
        {
            Console.WriteLine($"Slider {img.Id}: Buttons = {img.Buttons}");
        }

        return Ok(images);
    }

    [HttpPost]
    public async Task<ActionResult<SliderImage>> CreateSliderImage(CreateSliderImageDto dto)
    {
        var filteredButtons = dto.Buttons?.Where(b => !string.IsNullOrEmpty(b.Text) && !string.IsNullOrEmpty(b.Url)).ToList();
        var sliderImage = new SliderImage
        {
            ImageUrl = dto.ImageUrl,
            Title = dto.Title,
            Description = dto.Description,
            Order = dto.Order,
            Buttons = filteredButtons != null && filteredButtons.Any() ? System.Text.Json.JsonSerializer.Serialize(filteredButtons) : null,
            CreatedAt = DateTimeHelper.GetServerLocalTime()
        };

        _context.SliderImages.Add(sliderImage);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSliderImage), new { id = sliderImage.Id }, sliderImage);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SliderImage>> GetSliderImage(int id)
    {
        var sliderImage = await _context.SliderImages.FindAsync(id);

        if (sliderImage == null)
        {
            return NotFound();
        }

        return Ok(sliderImage);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSliderImage(int id, UpdateSliderImageDto dto)
    {
        var existing = await _context.SliderImages.FindAsync(id);

        if (existing == null)
        {
            return NotFound();
        }

        if (!string.IsNullOrEmpty(existing.ImageUrl) && existing.ImageUrl != dto.ImageUrl)
        {
            await _fileService.DeleteFileAsync(existing.ImageUrl);
        }

        var filteredButtons = dto.Buttons?.Where(b => !string.IsNullOrEmpty(b.Text) && !string.IsNullOrEmpty(b.Url)).ToList();
        existing.ImageUrl = dto.ImageUrl;
        existing.Title = dto.Title;
        existing.Description = dto.Description;
        existing.Order = dto.Order;
        existing.Buttons = filteredButtons != null && filteredButtons.Any() ? System.Text.Json.JsonSerializer.Serialize(filteredButtons) : null;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSliderImage(int id)
    {
        var sliderImage = await _context.SliderImages.FindAsync(id);

        if (sliderImage == null)
        {
            return NotFound();
        }

        if (!string.IsNullOrEmpty(sliderImage.ImageUrl))
        {
            await _fileService.DeleteFileAsync(sliderImage.ImageUrl);
        }

        _context.SliderImages.Remove(sliderImage);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
