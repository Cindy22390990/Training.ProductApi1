using Microsoft.EntityFrameworkCore;
using Training.ProductApi1.Data;
using Training.ProductApi1.Models;

namespace Training.ProductApi1.Repositories;

public class MaterialRepository :IMaterialRepository
{
    private readonly AppDbContext _context;
    public MaterialRepository(AppDbContext context)
    {
        _context = context;
    }
    public IQueryable<Material> GetAll()
    {
        return _context.Materials.AsNoTracking();
    }
    public async Task<Material?> GetByIdAsync(string id)
    {
        return await _context.Materials
            .FirstOrDefaultAsync(m => m.MaterialId == id);
    }
    public async Task<bool> ExistsAsync(string id)
    {
        return await _context.Materials
            .AnyAsync(m => m.MaterialId == id);
    }
    public async Task AddAsync(Material material) 
    {
        await _context.Materials.AddAsync(material);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Material material)
    {
        _context.Materials .Update(material);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(string id)
    {
        var material =await GetByIdAsync(id);
        if (material != null)
        { 
            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();
        }
        
    }

}
