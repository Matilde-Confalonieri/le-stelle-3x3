using Microsoft.EntityFrameworkCore;
using ThreeByThreeManager.Models;

namespace ThreeByThreeManager.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Player> Players => Set<Player>();
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<PlayerMatchStats> PlayerMatchStats => Set<PlayerMatchStats>();
    public DbSet<PlaybookScheme> PlaybookSchemes => Set<PlaybookScheme>();
    public DbSet<Shot> Shots => Set<Shot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlayerMatchStats>()
            .HasOne(s => s.Player)
            .WithMany(p => p.MatchStats)
            .HasForeignKey(s => s.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PlayerMatchStats>()
            .HasOne(s => s.Match)
            .WithMany(m => m.PlayerStats)
            .HasForeignKey(s => s.MatchId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PlayerMatchStats>()
            .HasIndex(s => new { s.PlayerId, s.MatchId })
            .IsUnique();

        modelBuilder.Entity<Shot>()
            .HasOne(s => s.Player)
            .WithMany(p => p.Shots)
            .HasForeignKey(s => s.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Shot>()
            .HasOne(s => s.Match)
            .WithMany(m => m.Shots)
            .HasForeignKey(s => s.MatchId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Shot>()
            .HasIndex(s => new { s.PlayerId, s.MatchId });
    }
}
