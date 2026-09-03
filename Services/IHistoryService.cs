using Training.ProductApi1.Models;

namespace Training.ProductApi1.Services;


public interface IHistoryService
{
    Task<List<History>> GetAllAsync();

    Task<History?> GetByIdAsync(int id);
}