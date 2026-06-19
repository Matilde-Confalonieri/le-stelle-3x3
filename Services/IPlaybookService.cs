using ThreeByThreeManager.Models;

namespace ThreeByThreeManager.Services;

public interface IPlaybookService
{
    Task<List<PlaybookScheme>> GetAllAsync();
    Task<List<PlaybookScheme>> GetByCategoryAsync(SchemeCategory category);
    Task<PlaybookScheme?> GetByIdAsync(int id);
    Task<PlaybookScheme> AddAsync(PlaybookScheme scheme);
    Task<PlaybookScheme> UpdateAsync(PlaybookScheme scheme);
    Task DeleteAsync(int id);
    Task<List<PlaybookScheme>> SearchAsync(string query);
}
