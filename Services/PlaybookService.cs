using Microsoft.EntityFrameworkCore;
using ThreeByThreeManager.Data;
using ThreeByThreeManager.Models;

namespace ThreeByThreeManager.Services;

public class PlaybookService : IPlaybookService
{
    private readonly IDbContextFactory<ApplicationDbContext> _factory;

    public PlaybookService(IDbContextFactory<ApplicationDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<List<PlaybookScheme>> GetAllAsync()
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.PlaybookSchemes.OrderBy(s => s.Category).ThenBy(s => s.Name).ToListAsync();
    }

    public async Task<List<PlaybookScheme>> GetByCategoryAsync(SchemeCategory category)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.PlaybookSchemes
            .Where(s => s.Category == category)
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<PlaybookScheme?> GetByIdAsync(int id)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.PlaybookSchemes.FindAsync(id);
    }

    public async Task<PlaybookScheme> AddAsync(PlaybookScheme scheme)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        ctx.PlaybookSchemes.Add(scheme);
        await ctx.SaveChangesAsync();
        return scheme;
    }

    public async Task<PlaybookScheme> UpdateAsync(PlaybookScheme scheme)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        var existing = await ctx.PlaybookSchemes.FindAsync(scheme.Id);
        if (existing == null) throw new InvalidOperationException("Schema non trovato");

        existing.Name = scheme.Name;
        existing.Category = scheme.Category;
        if (!string.IsNullOrEmpty(scheme.ImageUrl))
            existing.ImageUrl = scheme.ImageUrl;
        existing.Description = scheme.Description;
        existing.Notes = scheme.Notes;

        await ctx.SaveChangesAsync();
        return existing;
    }

    public async Task DeleteAsync(int id)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        var scheme = await ctx.PlaybookSchemes.FindAsync(id);
        if (scheme != null)
        {
            if (!string.IsNullOrEmpty(scheme.ImageUrl))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", scheme.ImageUrl.TrimStart('/'));
                if (File.Exists(filePath)) File.Delete(filePath);
            }
            ctx.PlaybookSchemes.Remove(scheme);
            await ctx.SaveChangesAsync();
        }
    }

    public async Task<List<PlaybookScheme>> SearchAsync(string query)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.PlaybookSchemes
            .Where(s => s.Name.Contains(query) || s.Description.Contains(query))
            .OrderBy(s => s.Name)
            .ToListAsync();
    }
}
