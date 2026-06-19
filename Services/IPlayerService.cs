using ThreeByThreeManager.Models;

namespace ThreeByThreeManager.Services;

public interface IPlayerService
{
    Task<List<Player>> GetAllAsync();
    Task<Player?> GetByIdAsync(int id);
    Task<Player> AddAsync(Player player);
    Task<Player> UpdateAsync(Player player);
    Task DeleteAsync(int id);
    Task<List<Player>> SearchAsync(string query);
}
