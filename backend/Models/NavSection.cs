using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using backend.Utils;

namespace backend.Models;

public enum NavSectionType
{
    Link = 0,       // Простая ссылка
    Dropdown = 1    // Выпадающее меню
}

public enum NavItemType
{
    InternalLink = 0,   // Внутренняя ссылка (/news, /events)
    ExternalLink = 1,   // Внешняя ссылка (https://...)
    CustomPage = 2      // Ссылка на кастомную страницу
}

public class NavSection
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Icon { get; set; }  // FontAwesome класс: "faHome", "faInfoCircle"

    public int Order { get; set; } = 0;

    public bool IsPublished { get; set; } = true;

    public NavSectionType Type { get; set; } = NavSectionType.Link;

    // Для Type = Link
    [StringLength(500)]
    public string? Url { get; set; }  // /news или https://external.com

    public bool IsExternal { get; set; } = false;

    public bool OpenInNewTab { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();

    public DateTime UpdatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();

    // Для Type = Dropdown
    public ICollection<NavSectionItem> Items { get; set; } = new List<NavSectionItem>();
}

public class NavSectionItem
{
    public int Id { get; set; }

    public int SectionId { get; set; }

    [JsonIgnore]
    public NavSection Section { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Icon { get; set; }

    public int Order { get; set; } = 0;

    public bool IsPublished { get; set; } = true;

    public NavItemType Type { get; set; } = NavItemType.InternalLink;

    // Для InternalLink / ExternalLink
    [StringLength(500)]
    public string? Url { get; set; }

    // Для CustomPage
    public int? CustomPageId { get; set; }
    public CustomPage? CustomPage { get; set; }

    public bool OpenInNewTab { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();
}