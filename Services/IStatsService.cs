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
    Task<List<Shot>> GetShotsByPlayerAsync(int playerId, int? matchId = null);
    Task<List<Shot>> GetShotsByMatchAsync(int matchId);
    Task<ShootingPercentages> GetShootingPercentagesAsync(int playerId, int? matchId = null);
    Task<Shot> AddShotAsync(Shot shot);
    Task DeleteShotAsync(int id);
}

public class ShootingPercentages
{
    public int FieldGoalsMade { get; set; }
    public int FieldGoalsAttempted { get; set; }
    public int ThreePointersMade { get; set; }
    public int ThreePointersAttempted { get; set; }
    public int TwoPointersMade { get; set; }
    public int TwoPointersAttempted { get; set; }
    public int FreeThrowsMade { get; set; }
    public int FreeThrowsAttempted { get; set; }
    public double FieldGoalPct => FieldGoalsAttempted > 0 ? FieldGoalsMade / (double)FieldGoalsAttempted * 100 : 0;
    public double ThreePointPct => ThreePointersAttempted > 0 ? ThreePointersMade / (double)ThreePointersAttempted * 100 : 0;
    public double TwoPointPct => TwoPointersAttempted > 0 ? TwoPointersMade / (double)TwoPointersAttempted * 100 : 0;
    public double FreeThrowPct => FreeThrowsAttempted > 0 ? FreeThrowsMade / (double)FreeThrowsAttempted * 100 : 0;
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
