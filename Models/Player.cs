using System.ComponentModel.DataAnnotations;

namespace ThreeByThreeManager.Models;

public class Player
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Il nome è obbligatorio")]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Il cognome è obbligatorio")]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Range(1, 99, ErrorMessage = "Il numero maglia deve essere tra 1 e 99")]
    public int JerseyNumber { get; set; }

    [MaxLength(30)]
    public string Role { get; set; } = string.Empty;

    [MaxLength(500000)]
    public string PhotoUrl { get; set; } = string.Empty;

    public List<PlayerMatchStats> MatchStats { get; set; } = new();

    public string FullName => $"{FirstName} {LastName}";
    public string DisplayRole => $"#{JerseyNumber}";
}
