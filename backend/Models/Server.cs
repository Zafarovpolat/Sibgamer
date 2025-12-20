using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Utils;

namespace backend.Models;

public class Server
{
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string IpAddress { get; set; } = string.Empty;

    [Required]
    public int Port { get; set; }

    [Column("rcon_password")]
    [StringLength(100)]
    public string? RconPassword { get; set; }

    public string MapName { get; set; } = string.Empty;

    public int CurrentPlayers { get; set; } = 0;

    public int MaxPlayers { get; set; } = 32;

    public bool IsOnline { get; set; } = false;

    public DateTime LastChecked { get; set; } = DateTimeHelper.GetServerLocalTime();
}
