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

        await SeedTestDataAsync(context);
    }

    public static async Task SeedTestDataAsync(ApplicationDbContext context)
    {
        if (await context.PlayerMatchStats.AnyAsync()) return;

        var players = await context.Players.OrderBy(p => p.Id).ToListAsync();
        var matches = await context.Matches.OrderBy(m => m.DateTime).ToListAsync();
        if (players.Count < 4 || matches.Count < 3) return;

        var m1 = matches[0]; m1.Status = MatchStatus.Played; m1.OurPoints = 15; m1.TheirPoints = 12;
        var m2 = matches[1]; m2.Status = MatchStatus.Played; m2.OurPoints = 21; m2.TheirPoints = 9;
        var m3 = matches[2]; m3.Status = MatchStatus.Played; m3.OurPoints = 18; m3.TheirPoints = 19;

        var stats = new List<PlayerMatchStats>
        {
            new() { PlayerId = players[0].Id, MatchId = m1.Id, Points = 6, Rebounds = 2, Assists = 3, Steals = 1, Blocks = 1, Fouls = 1 },
            new() { PlayerId = players[1].Id, MatchId = m1.Id, Points = 4, Rebounds = 3, Assists = 2, Steals = 2, Blocks = 0, Fouls = 2 },
            new() { PlayerId = players[2].Id, MatchId = m1.Id, Points = 3, Rebounds = 1, Assists = 4, Steals = 1, Blocks = 0, Fouls = 1 },
            new() { PlayerId = players[3].Id, MatchId = m1.Id, Points = 2, Rebounds = 4, Assists = 0, Steals = 0, Blocks = 2, Fouls = 1 },

            new() { PlayerId = players[0].Id, MatchId = m2.Id, Points = 9, Rebounds = 3, Assists = 2, Steals = 2, Blocks = 1, Fouls = 0 },
            new() { PlayerId = players[1].Id, MatchId = m2.Id, Points = 5, Rebounds = 1, Assists = 5, Steals = 1, Blocks = 0, Fouls = 1 },
            new() { PlayerId = players[2].Id, MatchId = m2.Id, Points = 4, Rebounds = 4, Assists = 1, Steals = 1, Blocks = 1, Fouls = 2 },
            new() { PlayerId = players[3].Id, MatchId = m2.Id, Points = 3, Rebounds = 2, Assists = 0, Steals = 0, Blocks = 0, Fouls = 0 },

            new() { PlayerId = players[0].Id, MatchId = m3.Id, Points = 8, Rebounds = 2, Assists = 3, Steals = 1, Blocks = 0, Fouls = 2 },
            new() { PlayerId = players[1].Id, MatchId = m3.Id, Points = 5, Rebounds = 3, Assists = 4, Steals = 2, Blocks = 0, Fouls = 3 },
            new() { PlayerId = players[2].Id, MatchId = m3.Id, Points = 3, Rebounds = 1, Assists = 1, Steals = 1, Blocks = 1, Fouls = 1 },
            new() { PlayerId = players[3].Id, MatchId = m3.Id, Points = 2, Rebounds = 5, Assists = 0, Steals = 0, Blocks = 2, Fouls = 2 },
        };

        context.PlayerMatchStats.AddRange(stats);
        await context.SaveChangesAsync();
    }
}
