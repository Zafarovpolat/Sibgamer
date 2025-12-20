using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SliderController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SliderController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetSliderImages()
    {
        var images = await _context.SliderImages
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        Console.WriteLine($"Returning {images.Count} slider images for public");
        foreach (var img in images)
        {
            Console.WriteLine($"Slider {img.Id}: Buttons = {img.Buttons}");
        }

        return Ok(images);
    }
}
