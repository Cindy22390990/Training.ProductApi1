using Training.ProductApi1.Models;
namespace Training.ProductApi1.Repositories;

public interface IHistoryRepository
{
    IQueryable<History> GetAll();

    Task<History?> GetByIdAsync(int id);
    Task AddAsync(History history);
}
