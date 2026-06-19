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
            new() { FirstName = "Arianna", LastName = "Panceri", JerseyNumber = 20, Role = "Play" },
            new() { FirstName = "Iris", LastName = "Camporeale", JerseyNumber = 41, Role = "Lungo" },
            new() { FirstName = "Matilde", LastName = "Confalonieri", JerseyNumber = 40, Role = "Lungo" },
            new() { FirstName = "Valentina", LastName = "Russica", JerseyNumber = 5, Role = "Play" },
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
                Tournament = "Sunset",
            },
            new()
            {
                DateTime = TournamentDate.ToDateTime(new TimeOnly(17, 45)),
                Location = "Campo B",
                Opponent = "Ducks",
                Type = MatchType.Group,
                Status = MatchStatus.Scheduled,
                Tournament = "Sunset",
            },
            new()
            {
                DateTime = TournamentDate.ToDateTime(new TimeOnly(19, 15)),
                Location = "Campo A",
                Opponent = "Abbelle Così",
                Type = MatchType.Group,
                Status = MatchStatus.Scheduled,
                Tournament = "Sunset",
            },
            new()
            {
                DateTime = TournamentDate.ToDateTime(new TimeOnly(20, 15)),
                Location = "Campo A",
                Opponent = "Da Definire",
                Type = MatchType.Semi,
                Status = MatchStatus.Scheduled,
                Tournament = "Sunset",
            },
            new()
            {
                DateTime = TournamentDate.ToDateTime(new TimeOnly(20, 15)),
                Location = "Campo B",
                Opponent = "Da Definire",
                Type = MatchType.Semi,
                Status = MatchStatus.Scheduled,
                Tournament = "Sunset",
            },
        };

        context.Matches.AddRange(matches);
        await context.SaveChangesAsync();
    }
}
