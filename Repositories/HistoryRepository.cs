using Microsoft.EntityFrameworkCore;
using Training.ProductApi1.Data;
using Training.ProductApi1.Models;

namespace Training.ProductApi1.Repositories;

public class HistoryRepository:IHistoryRepository
{
    private readonly AppDbContext _context;
    public HistoryRepository(AppDbContext context)
    {
        _context = context;
    }
    public IQueryable<History> GetAll()
    {
        return _context.Histories;
    }


    public async Task<History?> GetByIdAsync(int id)
    {
        return await _context.Histories
            .FirstOrDefaultAsync(h => h.Id == id);
    }
    public async Task AddAsync(History history)
    {
        await _context.Histories.AddAsync(history);
        await _context.SaveChangesAsync();
    }
        
}
