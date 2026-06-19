using Microsoft.EntityFrameworkCore;
using ThreeByThreeManager.Data;
using ThreeByThreeManager.Models;

namespace ThreeByThreeManager.Services;

public class PlayerService : IPlayerService
{
    private readonly IDbContextFactory<ApplicationDbContext> _factory;
    public PlayerService(IDbContextFactory<ApplicationDbContext> factory) { _factory = factory; }

    public async Task<List<Player>> GetAllAsync()
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.Players.Include(p => p.MatchStats).OrderBy(p => p.JerseyNumber).ToListAsync();
    }

    public async Task<Player?> GetByIdAsync(int id)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.Players.Include(p => p.MatchStats).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Player> AddAsync(Player player)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        ctx.Players.Add(player);
        await ctx.SaveChangesAsync();
        return player;
    }

    public async Task<Player> UpdateAsync(Player player)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        var existing = await ctx.Players.FindAsync(player.Id);
        if (existing == null) throw new InvalidOperationException("Giocatore non trovato");
        existing.FirstName = player.FirstName;
        existing.LastName = player.LastName;
        existing.JerseyNumber = player.JerseyNumber;
        existing.Role = player.Role;
        existing.PhotoUrl = player.PhotoUrl;
        await ctx.SaveChangesAsync();
        return existing;
    }

    public async Task DeleteAsync(int id)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        var player = await ctx.Players.FindAsync(id);
        if (player != null) { ctx.Players.Remove(player); await ctx.SaveChangesAsync(); }
    }

    public async Task<List<Player>> SearchAsync(string query)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.Players.Include(p => p.MatchStats)
            .Where(p => p.FirstName.Contains(query) || p.LastName.Contains(query))
            .ToListAsync();
    }
}
