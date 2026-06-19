using Microsoft.EntityFrameworkCore;
using ThreeByThreeManager.Models;
using MatchType = ThreeByThreeManager.Models.MatchType;
using MatchStatus = ThreeByThreeManager.Models.MatchStatus;
using SchemeCategory = ThreeByThreeManager.Models.SchemeCategory;

namespace ThreeByThreeManager.Data;

public static class DbSeeder
{
    private static readonly DateOnly TournamentDate = new(2026, 6, 20);

    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Players.AnyAsync()) return;

        var players = new List<Player>
        {
            new() { FirstName = "Arianna", LastName = "Panceri", JerseyNumber = 1 },
            new() { FirstName = "Iris", LastName = "Camporeale", JerseyNumber = 5 },
            new() { FirstName = "Matilde", LastName = "Confalonieri", JerseyNumber = 5 },
            new() { FirstName = "Valentina", LastName = "Russica", JerseyNumber = 2 },
        };

        context.Players.AddRange(players);
        await context.SaveChangesAsync();

        var matches = new List<Match>
        {
            new()
            {
                DateTime = TournamentDate.ToDateTime(new TimeOnly(17, 15)),
                Location = "Campo B",
                Opponent = "Le Bimbe di Sele",
                Type = MatchType.Group,
                Status = MatchStatus.Scheduled,
            },
            new()
            {
                DateTime = TournamentDate.ToDateTime(new TimeOnly(17, 45)),
                Location = "Campo B",
                Opponent = "Ducks",
                Type = MatchType.Group,
                Status = MatchStatus.Scheduled,
            },
            new()
            {
                DateTime = TournamentDate.ToDateTime(new TimeOnly(19, 15)),
                Location = "Campo A",
                Opponent = "Abbelle Così",
                Type = MatchType.Group,
                Status = MatchStatus.Scheduled,
            },
            new()
            {
                DateTime = TournamentDate.ToDateTime(new TimeOnly(20, 15)),
                Location = "Campo A",
                Opponent = "Da Definire",
                Type = MatchType.Semi,
                Status = MatchStatus.Scheduled,
            },
            new()
            {
                DateTime = TournamentDate.ToDateTime(new TimeOnly(20, 15)),
                Location = "Campo B",
                Opponent = "Da Definire",
                Type = MatchType.Semi,
                Status = MatchStatus.Scheduled,
            },
        };

        context.Matches.AddRange(matches);
        await context.SaveChangesAsync();

        var schemes = new List<PlaybookScheme>
        {
            new()
            {
                Name = "Pick & Roll Centrale",
                Category = SchemeCategory.Offense,
                Description = "Schema di attacco con pick and roll centrale. Il playmaker chiama il blocco del lungo al centro del campo. Dopo il blocco, il lungo rolla a canestro mentre il playmaker può attaccare o scaricare.",
                Notes = "Efficace contro difese statiche."
            },
            new()
            {
                Name = "Difesa a Uomo Pressing",
                Category = SchemeCategory.Defense,
                Description = "Difesa a uomo aggressiva su tutto il campo. Pressione costante sul portatore di palla, raddoppi sulle rimesse e aiuto immediato sui cambi.",
                Notes = "Ideale quando si è in svantaggio o nei momenti decisivi."
            },
            new()
            {
                Name = "Rimessa dal Fondo",
                Category = SchemeCategory.Inbound,
                Description = "Schema per rimessa dal fondo dopo canestro subìto. Due giocatori bloccano per liberare il playmaker.",
                Notes = "Variante A: ricezione playmaker. Variante B: passaggio lungo diretto."
            },
            new()
            {
                Name = "Gioco da Sotto",
                Category = SchemeCategory.Special,
                Description = "Schema per situazioni di confusione sotto canestro. Blocchi incrociati per liberare il tiratore sull'arco.",
                Notes = "Ultimo possesso con pochi secondi sul cronometro."
            },
        };

        context.PlaybookSchemes.AddRange(schemes);
        await context.SaveChangesAsync();
    }
}
