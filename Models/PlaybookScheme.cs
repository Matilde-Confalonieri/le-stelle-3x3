using System.ComponentModel.DataAnnotations;

namespace ThreeByThreeManager.Models;

public class PlaybookScheme
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Il nome è obbligatorio")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public SchemeCategory Category { get; set; } = SchemeCategory.Offense;

    [MaxLength(500000)]
    public string ImageUrl { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;

    public string CategoryDisplay => Category switch
    {
        SchemeCategory.Offense => "Attacco",
        SchemeCategory.Defense => "Difesa",
        SchemeCategory.Inbound => "Rimessa",
        SchemeCategory.Special => "Situazione Speciale",
        _ => "Attacco"
    };
}
