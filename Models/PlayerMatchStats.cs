using System.ComponentModel.DataAnnotations;

namespace ThreeByThreeManager.Models;

public class PlayerMatchStats
{
    public int Id { get; set; }

    [Required]
    public int PlayerId { get; set; }

    public Player Player { get; set; } = null!;

    [Required]
    public int MatchId { get; set; }

    public Match Match { get; set; } = null!;

    [Range(0, 100)]
    public int Points { get; set; }

    [Range(0, 100)]
    public int Rebounds { get; set; }

    [Range(0, 100)]
    public int Assists { get; set; }

    [Range(0, 100)]
    public int Steals { get; set; }

    [Range(0, 100)]
    public int Blocks { get; set; }

    [Range(0, 100)]
    public int Fouls { get; set; }

    public double Evaluation => Points + Rebounds + Assists + Steals + Blocks - Fouls;
}
