using Training.ProductApi1.Models;
namespace Training.ProductApi1.Repositories;

public interface IProductRepository
{
    IQueryable<Product> GetAll();
    Task<Product?> GetByIdAsync(string id);//查單筆
    Task<bool> ExistsAsync(string id);
    Task AddAsync(Product product);//新增
    Task UpdateAsync(Product product);//修改
    Task DeleteAsync(string id);//刪除
}
