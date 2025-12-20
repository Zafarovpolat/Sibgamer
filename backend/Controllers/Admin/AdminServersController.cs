using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services;
using backend.Utils;

namespace backend.Controllers;

[ApiController]
[Route("api/admin/servers")]
[Authorize(Roles = "Admin")]
public class AdminServersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IServerQueryService _queryService;

    public AdminServersController(ApplicationDbContext context, IServerQueryService queryService)
    {
        _context = context;
        _queryService = queryService;
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
                IsOnline = s.IsOnline
            })
            .ToListAsync();

        return Ok(servers);
    }

    [HttpPost]
    public async Task<ActionResult<ServerDto>> CreateServer(CreateServerDto dto)
    {
        var queryResult = await _queryService.QueryServerAsync(dto.IpAddress, dto.Port);

        var server = new Server
        {
            Name = queryResult?.ServerName ?? dto.Name ?? $"{dto.IpAddress}:{dto.Port}",
            IpAddress = dto.IpAddress,
            Port = dto.Port,
            MapName = queryResult?.Map ?? dto.MapName ?? "unknown",
            MaxPlayers = queryResult?.MaxPlayers ?? dto.MaxPlayers ?? 32,
            CurrentPlayers = queryResult?.Players ?? 0,
            IsOnline = queryResult?.IsOnline ?? false,
            LastChecked = DateTimeHelper.GetServerLocalTime()
        };

        server.RconPassword = dto.RconPassword;

        _context.Servers.Add(server);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetServer), new { id = server.Id }, new ServerDto
        {
            Id = server.Id,
            Name = server.Name,
            IpAddress = server.IpAddress,
            Port = server.Port,
            MapName = server.MapName,
            CurrentPlayers = server.CurrentPlayers,
            MaxPlayers = server.MaxPlayers,
            IsOnline = server.IsOnline
            ,RconPasswordSet = !string.IsNullOrEmpty(server.RconPassword)
        });
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
            IsOnline = server.IsOnline
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateServer(int id, CreateServerDto dto)
    {
        var server = await _context.Servers.FindAsync(id);

        if (server == null)
        {
            return NotFound();
        }

        server.IpAddress = dto.IpAddress;
        server.Port = dto.Port;
        server.RconPassword = dto.RconPassword; 

        var queryResult = await _queryService.QueryServerAsync(dto.IpAddress, dto.Port);
        
        if (queryResult != null)
        {
            server.Name = queryResult.ServerName;
            server.MapName = queryResult.Map;
            server.CurrentPlayers = queryResult.Players;
            server.MaxPlayers = queryResult.MaxPlayers;
            server.IsOnline = queryResult.IsOnline;
            server.LastChecked = DateTimeHelper.GetServerLocalTime();
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteServer(int id)
    {
        var server = await _context.Servers.FindAsync(id);

        if (server == null)
        {
            return NotFound();
        }

        _context.Servers.Remove(server);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{id}/refresh")]
    public async Task<ActionResult<ServerDto>> RefreshServer(int id)
    {
        var server = await _context.Servers.FindAsync(id);

        if (server == null)
        {
            return NotFound();
        }

        var queryResult = await _queryService.QueryServerAsync(server.IpAddress, server.Port);

        if (queryResult != null)
        {
            server.Name = queryResult.ServerName;
            server.MapName = queryResult.Map;
            server.CurrentPlayers = queryResult.Players;
            server.MaxPlayers = queryResult.MaxPlayers;
            server.IsOnline = queryResult.IsOnline;
            server.LastChecked = DateTimeHelper.GetServerLocalTime();

            await _context.SaveChangesAsync();
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
            IsOnline = server.IsOnline
            ,RconPasswordSet = !string.IsNullOrEmpty(server.RconPassword)
        });
    }
}
