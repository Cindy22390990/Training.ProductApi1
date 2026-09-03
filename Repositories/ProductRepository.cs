using Microsoft.EntityFrameworkCore;
using Training.ProductApi1.Data;
using Training.ProductApi1.Models;

namespace Training.ProductApi1.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    public ProductRepository(AppDbContext context) 
    {
        _context = context;
    }
    public IQueryable<Product> GetAll()
    {
        return _context.Products;
    }//查詢全部 Product
    public async Task<Product?> GetByIdAsync(string id)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.ProductId == id);
    }//查單筆 Product
    public async Task<bool> ExistsAsync(string id)
    {
        return await _context.Products
            .AnyAsync(p => p.ProductId == id);
    }
    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);

        await _context.SaveChangesAsync();
    }//新增 Product
    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);

        await _context.SaveChangesAsync();
    }//修改 Product
    public async Task DeleteAsync(string id)
    {
        var product = await GetByIdAsync(id);

        if (product != null)
        {
            _context.Products.Remove(product);

            await _context.SaveChangesAsync();
        }
    }//刪除 Product
}
