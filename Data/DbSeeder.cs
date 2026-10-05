using Microsoft.EntityFrameworkCore;
using ThreeByThreeManager.Models;
using MatchType = ThreeByThreeManager.Models.MatchType;
using MatchStatus = ThreeByThreeManager.Models.MatchStatus;
using SchemeCategory = ThreeByThreeManager.Models.SchemeCategory;

namespace ThreeByThreeManager.Data;

public static class DbSeeder
{
    public const int SeedVersion = 2;

    private static readonly Random Rng = new(20252026);

    public static async Task SeedAsync(ApplicationDbContext context, bool forceReseed = false)
    {
        var storedVersion = await context.SeedInfos.Select(s => s.Version).FirstOrDefaultAsync();
        var reseed = forceReseed || storedVersion != SeedVersion;

        if (reseed)
        {
            context.Shots.RemoveRange(context.Shots);
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

        var players = BuildPlayers();
        context.Players.AddRange(players);
        await context.SaveChangesAsync();

        var playedMatches = new List<Match>
        {
            new() { DateTime = Slot(-24, 20, 30), Location = "Casa",     Opponent = "Famila Wuber Schio",              Type = MatchType.Group,   Status = MatchStatus.Played, Tournament = "Serie A1", Notes = "Esordio convincente in difesa, 10 stoppate di squadra." },
            new() { DateTime = Slot(-19, 18, 0),  Location = "Trasferta", Opponent = "Umana Reyer Venezia",            Type = MatchType.Group,   Status = MatchStatus.Played, Tournament = "Serie A1", Notes = "Sconfitta di misura, troppi tiri liberi concessi nel finale." },
            new() { DateTime = Slot(-14, 20, 0),  Location = "Casa",     Opponent = "Virtus Segafredo Bologna",       Type = MatchType.Group,   Status = MatchStatus.Played, Tournament = "Serie A1", Notes = "Vittoria nel big match, 45% da tre di squadra." },
            new() { DateTime = Slot(-9, 17, 30),  Location = "Trasferta", Opponent = "Geas Sesto San Giovanni",        Type = MatchType.Group,   Status = MatchStatus.Played, Tournament = "Serie A1", Notes = "Partita controllata, dominio a rimbalzo." },
            new() { DateTime = Slot(-5, 19, 15),  Location = "Casa",     Opponent = "La Molisana Magnolia Campobasso", Type = MatchType.Group,   Status = MatchStatus.Played, Tournament = "Serie A1", Notes = "K.o. dopo un supplementare, brutte percentuali dal campo." },
            new() { DateTime = Slot(-3, 20, 30),  Location = "Casa",     Opponent = "RMB Brixia Basket Brescia",      Type = MatchType.Quarter, Status = MatchStatus.Played, Tournament = "Serie A1", Notes = "Passate ai quarti con autorità, terza regular season chiusa al 2° posto." },
            new() { DateTime = Slot(-1, 20, 0),   Location = "Trasferta", Opponent = "Alama San Martino di Lupari",   Type = MatchType.Semi,    Status = MatchStatus.Played, Tournament = "Serie A1", Notes = "Semifinale vinta in volata, 22 assist di squadra." },
        };

        var upcomingMatches = new List<Match>
        {
            new() { DateTime = Slot(3, 20, 30), Location = "Casa", Opponent = "Da Definire",          Type = MatchType.Final, Status = MatchStatus.Scheduled, Tournament = "Serie A1", Notes = "Finale scudetto: avversario deciso dall'altra semifinale." },
            new() { DateTime = Slot(8, 18, 0),  Location = "Casa", Opponent = "Dinamo Banco Sassari", Type = MatchType.Group, Status = MatchStatus.Scheduled, Tournament = "Serie A1", Notes = "Amichevole di metà stagione, formazione mista." },
            new() { DateTime = Slot(14, 18, 0), Location = "Trasferta", Opponent = "E-Work Faenza",   Type = MatchType.Group, Status = MatchStatus.Scheduled, Tournament = "Serie A1", Notes = "Ripresa campionato dopo la pausa." },
        };

        context.Matches.AddRange(playedMatches);
        context.Matches.AddRange(upcomingMatches);
        await context.SaveChangesAsync();

        // Margini (positivi = sconfitta, negativi = vittoria) per determinare i punti subiti.
        var margins = new[] { -11, 5, -9, -15, 3, -12, -4 };
        var stats = new List<PlayerMatchStats>();
        var shots = new List<Shot>();

        for (var i = 0; i < playedMatches.Count; i++)
        {
            var match = playedMatches[i];
            var teamPoints = 0;

            foreach (var player in players)
            {
                var (stat, playerShots) = GenerateStatLine(player, match);
                stats.Add(stat);
                shots.AddRange(playerShots);
                teamPoints += stat.Points;
            }

            match.OurPoints = teamPoints;
            match.TheirPoints = Math.Max(0, teamPoints + margins[i]);
        }

        context.PlayerMatchStats.AddRange(stats);
        context.Shots.AddRange(shots);
        await context.SaveChangesAsync();

        if (await context.PlaybookSchemes.AnyAsync()) return;

        context.PlaybookSchemes.AddRange(new List<PlaybookScheme>
        {
            new() { Name = "Pick & Roll Alto", Category = SchemeCategory.Offense, Description = "Blocco alto della play con il pivot, lettura sulla difesa e uscita per il tiro o scarico.", Notes = "Se il difensore passa sotto, prendere il tiro da tre con fiducia." },
            new() { Name = "Horns Entry", Category = SchemeCategory.Offense, Description = "Doppio blocco in corno, taglio del guardia e post basso per l'ala forte.", Notes = "Ottimo contro le difese a uomo aggressive. Durata 8-10 secondi." },
            new() { Name = "Switch Tutto", Category = SchemeCategory.Defense, Description = "Cambio sistematico su tutti i blocchi con aiuto lato debole della pivot.", Notes = "Richiede comunicazione costante e lettura dei tagli backdoor." },
            new() { Name = "Zona 2-3", Category = SchemeCategory.Defense, Description = "Zona 2-3 con pressione sul palleggiatore e chiusura sulle ali.", Notes = "Efficace contro squadre con poco tiro da fuori." },
            new() { Name = "Rimessa da Fondo", Category = SchemeCategory.Inbound, Description = "Rimessa da fondo con doppio blocco e taglio diagonale dell'ala.", Notes = "Vietato il palleggio cieco: designare sempre il ricevitore." },
            new() { Name = "Rimessa Laterale", Category = SchemeCategory.Inbound, Description = "Rimessa laterale a 5 secondi con azione decisa verso il canestro.", Notes = "La play chiama, le altre si liberano con blocchi scalari." },
            new() { Name = "Ultimo Possesso", Category = SchemeCategory.Special, Description = "Gestione degli ultimi 24 secondi con vantaggio o parità: isolamento o recupero.", Notes = "Con parità e meno di 10 secondi preferire il tiro da tre con mano forte." },
        });

        await context.SaveChangesAsync();

        context.SeedInfos.RemoveRange(context.SeedInfos);
        context.SeedInfos.Add(new SeedInfo { Version = SeedVersion });
        await context.SaveChangesAsync();
    }

    private static List<Player> BuildPlayers() => new()
    {
        new() { FirstName = "Giorgia",   LastName = "Sottana",     JerseyNumber = 4,  Role = "Play" },
        new() { FirstName = "Arianna",   LastName = "Carangelo",   JerseyNumber = 3,  Role = "Play" },
        new() { FirstName = "Francesca", LastName = "Dotto",       JerseyNumber = 7,  Role = "Guardia" },
        new() { FirstName = "Cecilia",   LastName = "Zandalasini", JerseyNumber = 9,  Role = "Ala" },
        new() { FirstName = "Jasmine",   LastName = "Keys",        JerseyNumber = 5,  Role = "Ala" },
        new() { FirstName = "Martina",   LastName = "Bestagno",    JerseyNumber = 8,  Role = "Ala" },
        new() { FirstName = "Elisa",     LastName = "Penna",       JerseyNumber = 11, Role = "Ala Grande" },
        new() { FirstName = "Olbis",     LastName = "Futo Andrè",  JerseyNumber = 44, Role = "Ala Grande" },
        new() { FirstName = "Lorela",    LastName = "Cubaj",       JerseyNumber = 13, Role = "Pivot" },
        new() { FirstName = "Costanza",  LastName = "Verona",      JerseyNumber = 15, Role = "Pivot" },
    };

    private static (PlayerMatchStats stat, List<Shot> shots) GenerateStatLine(Player player, Match match)
    {
        var (fg3a, fg3m, fg2a, fg2m, fta, ftm) = ShootingSplits(player);

        var stats = new PlayerMatchStats
        {
            Player = player,
            Match = match,
            Points = 2 * fg2m + 3 * fg3m + ftm,
            Rebounds = RoleStat(player.Role, "Play", 2, 5, "Guardia", 2, 5, "Ala", 4, 8, "Ala Grande", 5, 10, "Pivot", 6, 11),
            Assists = RoleStat(player.Role, "Play", 4, 8, "Guardia", 2, 5, "Ala", 1, 4, "Ala Grande", 1, 3, "Pivot", 1, 3),
            Steals = Rng.Next(0, 4),
            Blocks = RoleStat(player.Role, "Play", 0, 2, "Guardia", 0, 2, "Ala", 0, 2, "Ala Grande", 1, 4, "Pivot", 1, 4),
            Fouls = Rng.Next(1, 5),
            FreeThrowsMade = ftm,
            FreeThrowsAttempted = fta,
        };

        var shots = new List<Shot>();
        for (var i = 0; i < fg2a; i++)
        {
            var (x, y) = TwoPointSpot();
            shots.Add(new Shot { Player = player, Match = match, X = x, Y = y, Made = i < fg2m, IsThree = false });
        }
        for (var i = 0; i < fg3a; i++)
        {
            var (x, y) = ThreePointSpot();
            shots.Add(new Shot { Player = player, Match = match, X = x, Y = y, Made = i < fg3m, IsThree = true });
        }

        return (stats, shots);
    }

    private static (int fg3a, int fg3m, int fg2a, int fg2m, int fta, int ftm) ShootingSplits(Player player)
    {
        int fg3a, fg2a, fta;
        double fg3pct, fg2pct;
        switch (player.Role)
        {
            case "Play":
                fg3a = 2 + Rng.Next(0, 3); fg2a = 4 + Rng.Next(0, 3); fta = 1 + Rng.Next(0, 2);
                fg3pct = 0.33 + Rng.NextDouble() * 0.10; fg2pct = 0.45 + Rng.NextDouble() * 0.10;
                break;
            case "Guardia":
                fg3a = 2 + Rng.Next(0, 3); fg2a = 3 + Rng.Next(0, 3); fta = 1 + Rng.Next(0, 2);
                fg3pct = 0.32 + Rng.NextDouble() * 0.10; fg2pct = 0.45 + Rng.NextDouble() * 0.10;
                break;
            case "Ala":
                fg3a = 2 + Rng.Next(0, 2); fg2a = 4 + Rng.Next(0, 3); fta = 1 + Rng.Next(0, 2);
                fg3pct = 0.34 + Rng.NextDouble() * 0.10; fg2pct = 0.48 + Rng.NextDouble() * 0.10;
                break;
            case "Ala Grande":
                fg3a = Rng.Next(0, 2); fg2a = 4 + Rng.Next(0, 3); fta = 1 + Rng.Next(0, 2);
                fg3pct = 0.30 + Rng.NextDouble() * 0.10; fg2pct = 0.50 + Rng.NextDouble() * 0.08;
                break;
            case "Pivot":
                fg3a = 0; fg2a = 5 + Rng.Next(0, 3); fta = 1 + Rng.Next(0, 2);
                fg3pct = 0.30; fg2pct = 0.52 + Rng.NextDouble() * 0.08;
                break;
            default:
                fg3a = 1; fg2a = 4; fta = 1;
                fg3pct = 0.32; fg2pct = 0.48;
                break;
        }

        var fg3m = (int)Math.Round(fg3a * fg3pct);
        var fg2m = (int)Math.Round(fg2a * fg2pct);
        var ftm = (int)Math.Round(fta * 0.78);
        return (fg3a, fg3m, fg2a, fg2m, fta, ftm);
    }

    private static int RoleStat(string role, string r1, int lo1, int hi1, string r2, int lo2, int hi2,
        string r3, int lo3, int hi3, string r4, int lo4, int hi4, string r5, int lo5, int hi5)
    {
        return role switch
        {
            _ when role == r1 => Rng.Next(lo1, hi1 + 1),
            _ when role == r2 => Rng.Next(lo2, hi2 + 1),
            _ when role == r3 => Rng.Next(lo3, hi3 + 1),
            _ when role == r4 => Rng.Next(lo4, hi4 + 1),
            _ when role == r5 => Rng.Next(lo5, hi5 + 1),
            _ => Rng.Next(1, 4),
        };
    }

    private static (double x, double y) TwoPointSpot()
    {
        if (Rng.NextDouble() < 0.55)
            return (5.0 + Rng.NextDouble() * 5.0, 0.2 + Rng.NextDouble() * 5.3);
        return (2.5 + Rng.NextDouble() * 10.0, 5.9 + Rng.NextDouble() * 0.7);
    }

    private static (double x, double y) ThreePointSpot()
    {
        if (Rng.NextDouble() < 0.35)
        {
            var left = Rng.NextDouble() < 0.5;
            var x = left ? 0.9 + Rng.NextDouble() * 1.6 : 12.5 + Rng.NextDouble() * 1.6;
            return (x, 6.7 + Rng.NextDouble() * 0.6);
        }
        return (2.5 + Rng.NextDouble() * 10.0, 8.3 + Rng.NextDouble() * 1.2);
    }
}
