using System.ComponentModel.DataAnnotations;

namespace ThreeByThreeManager.Models;

public class Match
{
    public int Id { get; set; }

    [Required]
    public DateTime DateTime { get; set; } = DateTime.Today.AddDays(1).AddHours(18);

    [Required(ErrorMessage = "Il campo è obbligatorio")]
    [MaxLength(100)]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "L'avversario è obbligatorio")]
    [MaxLength(100)]
    public string Opponent { get; set; } = string.Empty;

    public MatchType Type { get; set; } = MatchType.Group;

    public MatchStatus Status { get; set; } = MatchStatus.Scheduled;

    public int OurPoints { get; set; }

    public int TheirPoints { get; set; }

    [MaxLength(100)]
    public string Tournament { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Notes { get; set; } = string.Empty;

    public List<PlayerMatchStats> PlayerStats { get; set; } = new();

    public bool IsVictory => Status == MatchStatus.Played && OurPoints > TheirPoints;

    public string TypeDisplay => Type switch
    {
        MatchType.Group => "Girone",
        MatchType.Quarter => "Quarti di Finale",
        MatchType.Semi => "Semifinale",
        MatchType.Final => "Finale",
        _ => "Girone"
    };

    public string TypeColor => Type switch
    {
        MatchType.Group => "#FF7A00",
        MatchType.Quarter => "#C0C0C0",
        MatchType.Semi => "#FFD700",
        MatchType.Final => "#FFCC00",
        _ => "#FF7A00"
    };

    public string LocationColor => Location switch
    {
        "Campo A" => "#00BFFF",
        "Campo B" => "#8A2BE2",
        _ => "#6C757D"
    };

    public string StatusDisplay => Status switch
    {
        MatchStatus.Scheduled => "In Programma",
        MatchStatus.Played => "Giocata",
        _ => "In Programma"
    };

    public string ResultDisplay =>
        Status == MatchStatus.Played ? $"{OurPoints} - {TheirPoints}" : "—";
}
