using Training.ProductApi1.Models;
using Training.ProductApi1.Models.DTOs;
namespace Training.ProductApi1.Services;

public interface IMaterialService
{
    Task<MaterialPageResultDto> GetPagedAsync(
    string? keyword,
    int pageIndex,
    int pageSize);
    Task<Material?> GetByIdAsync(string id);//查單筆
    Task AddAsync(Material material);//新增
    Task UpdateAsync(Material material);//修改
    Task DeleteAsync(string id);//刪除
    

}
