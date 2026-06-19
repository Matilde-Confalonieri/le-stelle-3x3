using ThreeByThreeManager.Models;

namespace ThreeByThreeManager.Services;

public interface IMatchService
{
    Task<List<Match>> GetAllAsync();
    Task<List<Match>> GetUpcomingAsync(int count = 5);
    Task<List<Match>> GetRecentAsync(int count = 5);
    Task<Match?> GetByIdAsync(int id);
    Task<Match> AddAsync(Match match);
    Task<Match> UpdateAsync(Match match);
    Task DeleteAsync(int id);
    Task UpdateScoreAsync(int matchId, int ourPoints, int theirPoints);
}
