using Microsoft.EntityFrameworkCore;
using ThreeByThreeManager.Models;
using MatchType = ThreeByThreeManager.Models.MatchType;
using MatchStatus = ThreeByThreeManager.Models.MatchStatus;
using SchemeCategory = ThreeByThreeManager.Models.SchemeCategory;

namespace ThreeByThreeManager.Data;

public static class DbSeeder
{
    private static readonly int[][][] StatLines =
    {
        new[]
        {
            new[] { 5, 3, 5, 2, 0, 1 }, new[] { 3, 2, 4, 1, 0, 2 }, new[] { 2, 1, 3, 2, 0, 1 },
            new[] { 2, 3, 2, 3, 1, 2 }, new[] { 4, 6, 1, 1, 3, 3 }, new[] { 3, 6, 0, 2, 1, 1 }
        },
        new[]
        {
            new[] { 4, 2, 4, 1, 0, 3 }, new[] { 2, 1, 3, 2, 0, 2 }, new[] { 2, 2, 2, 1, 0, 1 },
            new[] { 2, 2, 3, 0, 1, 2 }, new[] { 2, 5, 0, 1, 2, 4 }, new[] { 2, 4, 0, 0, 1, 2 }
        },
        new[]
        {
            new[] { 6, 4, 6, 3, 0, 1 }, new[] { 3, 3, 2, 1, 1, 2 }, new[] { 3, 2, 3, 4, 0, 1 },
            new[] { 2, 3, 2, 1, 0, 2 }, new[] { 3, 7, 1, 2, 3, 3 }, new[] { 4, 5, 1, 1, 1, 1 }
        },
        new[]
        {
            new[] { 3, 2, 3, 1, 0, 4 }, new[] { 2, 2, 1, 0, 1, 3 }, new[] { 1, 3, 1, 2, 0, 2 },
            new[] { 1, 2, 1, 0, 0, 3 }, new[] { 2, 4, 0, 1, 1, 4 }, new[] { 3, 4, 1, 0, 0, 3 }
        },
        new[]
        {
            new[] { 4, 3, 7, 2, 0, 2 }, new[] { 4, 4, 3, 2, 1, 1 }, new[] { 3, 2, 4, 2, 0, 2 },
            new[] { 2, 3, 2, 1, 0, 2 }, new[] { 2, 6, 0, 3, 2, 2 }, new[] { 3, 6, 0, 1, 2, 1 }
        },
        new[]
        {
            new[] { 5, 2, 5, 3, 0, 2 }, new[] { 2, 3, 4, 1, 0, 3 }, new[] { 4, 1, 2, 2, 1, 1 },
            new[] { 3, 2, 3, 0, 0, 1 }, new[] { 3, 5, 1, 2, 3, 3 }, new[] { 3, 5, 1, 1, 2, 2 }
        },
        new[]
        {
            new[] { 4, 3, 2, 0, 0, 4 }, new[] { 2, 2, 3, 1, 1, 3 }, new[] { 1, 2, 2, 3, 0, 2 },
            new[] { 2, 3, 2, 0, 0, 4 }, new[] { 3, 6, 0, 1, 2, 4 }, new[] { 3, 4, 0, 1, 0, 3 }
        },
        new[]
        {
            new[] { 5, 4, 6, 4, 0, 2 }, new[] { 2, 3, 3, 2, 1, 2 }, new[] { 3, 1, 3, 3, 0, 1 },
            new[] { 3, 2, 4, 1, 1, 2 }, new[] { 4, 6, 1, 2, 3, 3 }, new[] { 5, 5, 1, 1, 2, 2 }
        }
    };

    public static async Task SeedAsync(ApplicationDbContext context, bool forceReseed = false)
    {
        if (forceReseed)
        {
            context.PlayerMatchStats.RemoveRange(context.PlayerMatchStats);
            context.Matches.RemoveRange(context.Matches);
            context.Players.RemoveRange(context.Players);
            context.PlaybookSchemes.RemoveRange(context.PlaybookSchemes);
            await context.SaveChangesAsync();
        }

        if (await context.Players.AnyAsync()) return;

        var today = DateOnly.FromDateTime(DateTime.Today);

        DateTime Slot(int dayOffset, int hour, int minute) =>
            DateTime.SpecifyKind(
                today.AddDays(dayOffset).ToDateTime(new TimeOnly(hour, minute)),
                DateTimeKind.Utc);

        var players = new List<Player>
        {
            new() { FirstName = "Aurora", LastName = "Farabello", JerseyNumber = 7, Role = "Play" },
            new() { FirstName = "Giulia", LastName = "Cola", JerseyNumber = 11, Role = "Play" },
            new() { FirstName = "Vittoria", LastName = "Ibba", JerseyNumber = 4, Role = "Guardia" },
            new() { FirstName = "Beatrice", LastName = "Meroi", JerseyNumber = 18, Role = "Guardia" },
            new() { FirstName = "Cecilia", LastName = "Zandalasini", JerseyNumber = 23, Role = "Lungo" },
            new() { FirstName = "Kitija", LastName = "Laksa", JerseyNumber = 32, Role = "Lungo" },
        };

        context.Players.AddRange(players);
        await context.SaveChangesAsync();

        var playedMatches = new List<Match>
        {
            new()
            {
                DateTime = Slot(-18, 17, 15),
                Location = "Campo A",
                Opponent = "Famila Wuber Schio",
                Type = MatchType.Group,
                Status = MatchStatus.Played,
                OurPoints = 19,
                TheirPoints = 12,
                Tournament = "Sunset",
                Notes = "Esordio con doppia vittoria. Ottima pressione difensiva nel primo periodo.",
            },
            new()
            {
                DateTime = Slot(-15, 18, 45),
                Location = "Campo B",
                Opponent = "Umana Reyer Venezia",
                Type = MatchType.Group,
                Status = MatchStatus.Played,
                OurPoints = 14,
                TheirPoints = 16,
                Tournament = "Sunset",
                Notes = "Sconfitta per due punti. Tiri troppo concessi nel primo periodo.",
            },
            new()
            {
                DateTime = Slot(-11, 20, 0),
                Location = "Campo A",
                Opponent = "BCC Derthona Tortona",
                Type = MatchType.Group,
                Status = MatchStatus.Played,
                OurPoints = 21,
                TheirPoints = 13,
                Tournament = "Sunset",
                Notes = "Partita migliore del girone: 21 punti e zero timeout sprecati.",
            },
            new()
            {
                DateTime = Slot(-8, 17, 30),
                Location = "Campo C",
                Opponent = "Geas Sesto San Giovanni",
                Type = MatchType.Group,
                Status = MatchStatus.Played,
                OurPoints = 12,
                TheirPoints = 15,
                Tournament = "Sunset",
                Notes = "Turnover alti. Da rivedere la gestione della palla nel finale di gara.",
            },
            new()
            {
                DateTime = Slot(-5, 19, 15),
                Location = "Campo B",
                Opponent = "La Molisana Magnolia Campobasso",
                Type = MatchType.Group,
                Status = MatchStatus.Played,
                OurPoints = 18,
                TheirPoints = 11,
                Tournament = "Sunset",
                Notes = "Vittoria netta con 14 rimbalzi di squadra.",
            },
            new()
            {
                DateTime = Slot(-3, 20, 30),
                Location = "Campo A",
                Opponent = "RMB Brixia Basket Brescia",
                Type = MatchType.Quarter,
                Status = MatchStatus.Played,
                OurPoints = 20,
                TheirPoints = 18,
                Tournament = "Sunset",
                Notes = "Superata ai tiri liberi. Chiude la regular season al terzo posto.",
            },
            new()
            {
                DateTime = Slot(-2, 18, 0),
                Location = "Campo B",
                Opponent = "People Strategy Panthers Roseto",
                Type = MatchType.Quarter,
                Status = MatchStatus.Played,
                OurPoints = 15,
                TheirPoints = 18,
                Tournament = "Sunset",
                Notes = "Eliminate dai quarti. Cinque falli accumulate nel terzo periodo.",
            },
            new()
            {
                DateTime = Slot(-1, 19, 45),
                Location = "Campo A",
                Opponent = "Logiman Broni",
                Type = MatchType.Semi,
                Status = MatchStatus.Played,
                OurPoints = 22,
                TheirPoints = 20,
                Tournament = "Sunset",
                Notes = "Semifinale vinta al golden break. Dieci assist di squadra.",
            },
        };

        var upcomingMatches = new List<Match>
        {
            new()
            {
                DateTime = Slot(2, 17, 30),
                Location = "Campo B",
                Opponent = "Da Definire",
                Type = MatchType.Semi,
                Status = MatchStatus.Scheduled,
                Tournament = "Sunset",
                Notes = "Seconda semifinale: avversario definito in base al risultato di ieri.",
            },
            new()
            {
                DateTime = Slot(5, 20, 0),
                Location = "Campo A",
                Opponent = "Da Definire",
                Type = MatchType.Final,
                Status = MatchStatus.Scheduled,
                Tournament = "Sunset",
                Notes = "Finale del torneo Sunset.",
            },
            new()
            {
                DateTime = Slot(12, 18, 0),
                Location = "Campo B",
                Opponent = "Ladies Libertas Livorno",
                Type = MatchType.Group,
                Status = MatchStatus.Scheduled,
                Tournament = "Sunset",
                Notes = "Amichevole di chiusura stagione, formazione mista.",
            },
        };

        context.Matches.AddRange(playedMatches);
        context.Matches.AddRange(upcomingMatches);
        await context.SaveChangesAsync();

        var stats = new List<PlayerMatchStats>();
        for (var m = 0; m < StatLines.Length; m++)
        {
            for (var p = 0; p < players.Count; p++)
            {
                var line = StatLines[m][p];
                stats.Add(new PlayerMatchStats
                {
                    Player = players[p],
                    Match = playedMatches[m],
                    Points = line[0],
                    Rebounds = line[1],
                    Assists = line[2],
                    Steals = line[3],
                    Blocks = line[4],
                    Fouls = line[5],
                });
            }
        }

        context.PlayerMatchStats.AddRange(stats);
        await context.SaveChangesAsync();

        if (await context.PlaybookSchemes.AnyAsync()) return;

        context.PlaybookSchemes.AddRange(new List<PlaybookScheme>
        {
            new()
            {
                Name = "Iso Baseline",
                Category = SchemeCategory.Offense,
                Description = "Isolamento per la play in fondo. Blocco a due, swing e attacco di rimessa in tre tempi.",
                Notes = "Se il difensore sul primo aiuto chiude, uscire subito con il passaggio al blocco.",
            },
            new()
            {
                Name = "Orbit Hand-off",
                Category = SchemeCategory.Offense,
                Description = "Orbit a due per la guardia, hand-off e uscita in rollata verso il canale corto.",
                Notes = "Ottimo contro squadre che marcano individualmente. Durata massima 8 secondi.",
            },
            new()
            {
                Name = "Switch 1-2-3",
                Category = SchemeCategory.Defense,
                Description = "Cambio su tutto il perimetro con aiuto del centro sui movimenti senza palla.",
                Notes = "Attenzione ai due salti consecutivi: sempre coprire il lancio corto.",
            },
            new()
            {
                Name = "Zona 2-3 Whole-Court",
                Category = SchemeCategory.Defense,
                Description = "Zona 2-3 a tutto campo con pressione sul palleggiatore e drop sui lanciatori.",
                Notes = "Ottima contro le squadre che impostano molto. Richiede disciplina di rotazione.",
            },
            new()
            {
                Name = "Rimessa Baseline",
                Category = SchemeCategory.Inbound,
                Description = "Rimessa da fondo dopo canestro: doppio, blocco e taglio diagonale.",
                Notes = "Vietato il palleggio cieco. Prendere sempre un timeout se la pressione è alta.",
            },
            new()
            {
                Name = "Rimessa Sideline 5",
                Category = SchemeCategory.Inbound,
                Description = "Rimessa laterale a 5 secondi con azione decisa al check.",
                Notes = "Designare sempre il ricevitore: la play chiama, le altre si liberano.",
            },
            new()
            {
                Name = "Ultimo Possesso",
                Category = SchemeCategory.Special,
                Description = "Gestione degli ultimi 30 secondi con vantaggio o parità: attacco isolato o recupero.",
                Notes = "Con parità e meno di 10 secondi preferire il tiro da 2PT con mano forte.",
            },
        });

        await context.SaveChangesAsync();
    }
}