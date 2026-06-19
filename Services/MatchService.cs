using Microsoft.EntityFrameworkCore;
using ThreeByThreeManager.Data;
using ThreeByThreeManager.Models;

namespace ThreeByThreeManager.Services;

public class MatchService : IMatchService
{
    private readonly IDbContextFactory<ApplicationDbContext> _factory;

    public MatchService(IDbContextFactory<ApplicationDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<List<Match>> GetAllAsync()
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.Matches
            .Include(m => m.PlayerStats).ThenInclude(s => s.Player)
            .OrderByDescending(m => m.DateTime)
            .ToListAsync();
    }

    public async Task<List<Match>> GetUpcomingAsync(int count = 5)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.Matches
            .Include(m => m.PlayerStats).ThenInclude(s => s.Player)
            .Where(m => m.Status == MatchStatus.Scheduled)
            .OrderBy(m => m.DateTime)
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<Match>> GetRecentAsync(int count = 5)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.Matches
            .Include(m => m.PlayerStats).ThenInclude(s => s.Player)
            .Where(m => m.Status == MatchStatus.Played)
            .OrderByDescending(m => m.DateTime)
            .Take(count)
            .ToListAsync();
    }

    public async Task<Match?> GetByIdAsync(int id)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.Matches
            .Include(m => m.PlayerStats).ThenInclude(s => s.Player)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Match> AddAsync(Match match)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        if (match.OurPoints > 0 || match.TheirPoints > 0)
            match.Status = MatchStatus.Played;
        ctx.Matches.Add(match);
        await ctx.SaveChangesAsync();
        return match;
    }

    public async Task<Match> UpdateAsync(Match match)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        var existing = await ctx.Matches.FindAsync(match.Id);
        if (existing == null) throw new InvalidOperationException("Partita non trovata");

        existing.DateTime = match.DateTime;
        existing.Location = match.Location;
        existing.Opponent = match.Opponent;
        existing.Type = match.Type;
        existing.OurPoints = match.OurPoints;
        existing.TheirPoints = match.TheirPoints;
        existing.Notes = match.Notes;

        if (match.OurPoints > 0 || match.TheirPoints > 0)
            existing.Status = MatchStatus.Played;

        await ctx.SaveChangesAsync();
        return existing;
    }

    public async Task DeleteAsync(int id)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        var match = await ctx.Matches.FindAsync(id);
        if (match != null)
        {
            ctx.Matches.Remove(match);
            await ctx.SaveChangesAsync();
        }
    }

    public async Task UpdateScoreAsync(int matchId, int ourPoints, int theirPoints)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        var match = await ctx.Matches.FindAsync(matchId);
        if (match == null) throw new InvalidOperationException("Partita non trovata");

        match.OurPoints = ourPoints;
        match.TheirPoints = theirPoints;
        match.Status = MatchStatus.Played;
        await ctx.SaveChangesAsync();
    }
}
