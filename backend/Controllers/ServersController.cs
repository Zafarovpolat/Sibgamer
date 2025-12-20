using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ServersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServerDto>>> GetServers()
    {
        var servers = await _context.Servers
            .Select(s => new ServerDto
            {
                Id = s.Id,
                Name = s.Name,
                IpAddress = s.IpAddress,
                Port = s.Port,
                MapName = s.MapName,
                CurrentPlayers = s.CurrentPlayers,
                MaxPlayers = s.MaxPlayers,
                IsOnline = s.IsOnline,
                RconPasswordSet = !string.IsNullOrEmpty(s.RconPassword)
            })
            .ToListAsync();

        return Ok(servers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServerDto>> GetServer(int id)
    {
        var server = await _context.Servers.FindAsync(id);

        if (server == null)
        {
            return NotFound();
        }

        return Ok(new ServerDto
        {
            Id = server.Id,
            Name = server.Name,
            IpAddress = server.IpAddress,
            Port = server.Port,
            MapName = server.MapName,
            CurrentPlayers = server.CurrentPlayers,
            MaxPlayers = server.MaxPlayers,
            IsOnline = server.IsOnline,
            RconPasswordSet = !string.IsNullOrEmpty(server.RconPassword)
        });
    }
}
