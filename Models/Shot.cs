using System.ComponentModel.DataAnnotations;

namespace ThreeByThreeManager.Models;

public class Shot
{
    public int Id { get; set; }

    [Required]
    public int PlayerId { get; set; }

    public Player Player { get; set; } = null!;

    [Required]
    public int MatchId { get; set; }

    public Match Match { get; set; } = null!;

    /// <summary>Coordinata orizzontale in metri (0 = linea laterale sinistra, 15 = destra).</summary>
    public double X { get; set; }

    /// <summary>Coordinata verticale in metri dalla linea di fondo (0 = fondo, 14 = metà campo).</summary>
    public double Y { get; set; }

    public bool Made { get; set; }

    public bool IsThree { get; set; }

    public int Value => IsThree ? 3 : 2;
}
