using Microsoft.EntityFrameworkCore;
using Training.ProductApi1.Models;
using Training.ProductApi1.Repositories;

namespace Training.ProductApi1.Services;


public class HistoryService : IHistoryService
{
    private readonly IHistoryRepository _historyRepository;


    public HistoryService(
        IHistoryRepository historyRepository)
    {
        _historyRepository = historyRepository;
    }



    public async Task<List<History>> GetAllAsync()
    {
        return await _historyRepository
            .GetAll()
            .OrderByDescending(h => h.CreatedAt)
            .ToListAsync();
    }



    public async Task<History?> GetByIdAsync(int id)
    {
        return await _historyRepository
            .GetByIdAsync(id);
    }
}