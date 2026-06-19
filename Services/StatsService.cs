using Microsoft.EntityFrameworkCore;
using ThreeByThreeManager.Data;
using ThreeByThreeManager.Models;

namespace ThreeByThreeManager.Services;

public class StatsService : IStatsService
{
    private readonly IDbContextFactory<ApplicationDbContext> _factory;

    public StatsService(IDbContextFactory<ApplicationDbContext> factory) { _factory = factory; }

    public async Task<List<PlayerMatchStats>> GetStatsByMatchAsync(int matchId)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.PlayerMatchStats.Include(s => s.Player)
            .Where(s => s.MatchId == matchId).OrderByDescending(s => s.Points).ToListAsync();
    }

    public async Task<List<PlayerMatchStats>> GetStatsByPlayerAsync(int playerId)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.PlayerMatchStats.Include(s => s.Match)
            .Where(s => s.PlayerId == playerId).OrderByDescending(s => s.Match.DateTime).ToListAsync();
    }

    public async Task<List<PlayerMatchStats>> GetAllStatsAsync()
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.PlayerMatchStats.Include(s => s.Player).Include(s => s.Match)
            .OrderByDescending(s => s.Match.DateTime).ToListAsync();
    }

    public async Task<PlayerMatchStats?> GetByIdAsync(int id)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.PlayerMatchStats.Include(s => s.Player).Include(s => s.Match)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<PlayerMatchStats> AddOrUpdateStatsAsync(PlayerMatchStats stats)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        var existing = await ctx.PlayerMatchStats
            .FirstOrDefaultAsync(s => s.PlayerId == stats.PlayerId && s.MatchId == stats.MatchId);
        if (existing != null)
        {
            existing.Points = stats.Points; existing.Rebounds = stats.Rebounds;
            existing.Assists = stats.Assists; existing.Steals = stats.Steals;
            existing.Blocks = stats.Blocks; existing.Fouls = stats.Fouls;
        }
        else { ctx.PlayerMatchStats.Add(stats); existing = stats; }
        await ctx.SaveChangesAsync();
        return existing;
    }

    public async Task DeleteStatsAsync(int id)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        var stats = await ctx.PlayerMatchStats.FindAsync(id);
        if (stats != null) { ctx.PlayerMatchStats.Remove(stats); await ctx.SaveChangesAsync(); }
    }

    public async Task<PlayerMatchStats?> GetMatchMVPAsync(int matchId)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.PlayerMatchStats.Include(s => s.Player).Where(s => s.MatchId == matchId)
            .OrderByDescending(s => s.Points + s.Assists + s.Rebounds + s.Steals + s.Blocks)
            .FirstOrDefaultAsync();
    }

    public async Task<List<PlayerSeasonStats>> GetSeasonStatsAsync()
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        var stats = await ctx.PlayerMatchStats.Include(s => s.Player).Include(s => s.Match).ToListAsync();
        return stats.GroupBy(s => s.PlayerId).Select(g => new PlayerSeasonStats
        {
            PlayerId = g.Key, PlayerName = g.First().Player.FullName,
            JerseyNumber = g.First().Player.JerseyNumber, PhotoUrl = g.First().Player.PhotoUrl,
            GamesPlayed = g.Select(s => s.MatchId).Distinct().Count(),
            TotalPoints = g.Sum(s => s.Points), TotalRebounds = g.Sum(s => s.Rebounds),
            TotalAssists = g.Sum(s => s.Assists), TotalSteals = g.Sum(s => s.Steals),
            TotalBlocks = g.Sum(s => s.Blocks), TotalFouls = g.Sum(s => s.Fouls),
        }).ToList();
    }

    public async Task<List<ScorerRanking>> GetScorerRankingsAsync()
    {
        var seasonStats = await GetSeasonStatsAsync();
        return seasonStats.Select(s => new ScorerRanking
        {
            PlayerId = s.PlayerId, PlayerName = s.PlayerName,
            JerseyNumber = s.JerseyNumber, PhotoUrl = s.PhotoUrl,
            TotalPoints = s.TotalPoints, GamesPlayed = s.GamesPlayed,
        }).OrderByDescending(s => s.TotalPoints).ToList();
    }
}
