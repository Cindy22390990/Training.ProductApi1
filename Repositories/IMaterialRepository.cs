using Training.ProductApi1.Models;
using Training.ProductApi1.Models.DTOs;
namespace Training.ProductApi1.Repositories;

public interface IMaterialRepository
{
    IQueryable<Material> GetAll();
    Task<Material?> GetByIdAsync(string id);

    Task<bool> ExistsAsync(string id);

    Task AddAsync(Material material);

    Task UpdateAsync(Material material);

    Task DeleteAsync(string id);
}
