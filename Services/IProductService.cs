using Training.ProductApi1.Models;
using Training.ProductApi1.Models.DTOs;
namespace Training.ProductApi1.Services;

public interface IProductService
{
    
    Task<Product?> GetByIdAsync(string id);//查單筆
    Task AddAsync(Product product);//新增
    Task UpdateAsync(Product product);//修改
    Task DeleteAsync(string id);//刪除
    Task<ProductPageResultDto> GetPagedAsync(
    int pageIndex,
    int pageSize);

}
