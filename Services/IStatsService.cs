using ThreeByThreeManager.Models;

namespace ThreeByThreeManager.Services;

public interface IStatsService
{
    Task<List<PlayerMatchStats>> GetStatsByMatchAsync(int matchId);
    Task<List<PlayerMatchStats>> GetStatsByPlayerAsync(int playerId);
    Task<List<PlayerMatchStats>> GetAllStatsAsync();
    Task<PlayerMatchStats?> GetByIdAsync(int id);
    Task<PlayerMatchStats> AddOrUpdateStatsAsync(PlayerMatchStats stats);
    Task DeleteStatsAsync(int id);
    Task<PlayerMatchStats?> GetMatchMVPAsync(int matchId);
    Task<List<PlayerSeasonStats>> GetSeasonStatsAsync();
    Task<List<ScorerRanking>> GetScorerRankingsAsync();
}

public class PlayerSeasonStats
{
    public int PlayerId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public int JerseyNumber { get; set; }
    public string PhotoUrl { get; set; } = string.Empty;
    public int GamesPlayed { get; set; }
    public int TotalPoints { get; set; }
    public double AvgPoints => GamesPlayed > 0 ? (double)TotalPoints / GamesPlayed : 0;
    public int TotalRebounds { get; set; }
    public double AvgRebounds => GamesPlayed > 0 ? (double)TotalRebounds / GamesPlayed : 0;
    public int TotalAssists { get; set; }
    public double AvgAssists => GamesPlayed > 0 ? (double)TotalAssists / GamesPlayed : 0;
    public int TotalSteals { get; set; }
    public int TotalBlocks { get; set; }
    public int TotalFouls { get; set; }
    public double AvgEval => GamesPlayed > 0
        ? (TotalPoints + TotalRebounds + TotalAssists + TotalSteals + TotalBlocks - TotalFouls) / (double)GamesPlayed
        : 0;
}

public class ScorerRanking
{
    public int PlayerId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public int JerseyNumber { get; set; }
    public string PhotoUrl { get; set; } = string.Empty;
    public int TotalPoints { get; set; }
    public int GamesPlayed { get; set; }
    public double AvgPoints => GamesPlayed > 0 ? (double)TotalPoints / GamesPlayed : 0;
}
