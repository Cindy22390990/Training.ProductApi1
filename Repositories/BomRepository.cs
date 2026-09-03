using Microsoft.EntityFrameworkCore;
using Training.ProductApi1.Data;
using Training.ProductApi1.Models;

namespace Training.ProductApi1.Repositories;

public class BomRepository :IBomRepository
{
    private readonly AppDbContext _context;
    public BomRepository(AppDbContext context)
    {
        _context = context;
    }
    public IQueryable<Bom> GetAll()
    {
        return _context.Boms
           .Include(b => b.Product)
           .Include(b => b.Material);
    }//查詢全部 Bom
    public async Task<Bom?> GetByIdAsync(int id)
    {
        return await _context.Boms
            .Include(b => b.Product)
            .Include(b => b.Material)
            .FirstOrDefaultAsync(b => b.Id == id);
            
    }//查單筆 Bom
    public async Task AddAsync(Bom bom)
    {
        await _context.Boms.AddAsync(bom);

        await _context.SaveChangesAsync();
    }//新增 Bom
    public async Task UpdateAsync(Bom bom)
    {
        _context.Boms.Update(bom);

        await _context.SaveChangesAsync();
    }//修改 Bom
    public async Task DeleteAsync(int id)
    {
        var bom = await GetByIdAsync(id);

        if (bom != null)
        {
            _context.Boms.Remove(bom);

            await _context.SaveChangesAsync();
        }
    }//刪除 Bom
    public async Task<bool>ExistsByProductIdAsync(string productId)
    {
        return await _context.Boms
            .AnyAsync(b => b.ProductId == productId);
    }
    public async Task<bool> ExistsByMaterialIdAsync(string materialId)
    {
        return await _context.Boms
            .AnyAsync(b => b.MaterialId == materialId);
    }
    public async Task<bool> ExistsAsync(
    string productId,
    string materialId,
    int excludeId = 0)
    {
        var list = await _context.Boms.ToListAsync();

        Console.WriteLine("====== BOM ======");

        foreach (var item in list)
        {
            Console.WriteLine(
                $"{item.Id} {item.ProductId} {item.MaterialId}");
        }

        var result = await _context.Boms.AnyAsync(b =>
            b.ProductId == productId &&
            b.MaterialId == materialId &&
            b.Id != excludeId);

        Console.WriteLine($"Exists={result}");

        return result;
    }
}
