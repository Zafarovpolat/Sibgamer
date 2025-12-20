namespace backend.Models;

using backend.Utils;

public class SiteSetting
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; 
    public string Description { get; set; } = string.Empty; 
    public string DataType { get; set; } = "string";
    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();
    public DateTime UpdatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();
}
