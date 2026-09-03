using Training.ProductApi1.Models;
namespace Training.ProductApi1.Repositories;
    public interface IBomRepository
    {
        IQueryable<Bom> GetAll();

        Task<Bom?> GetByIdAsync(int id);

        Task AddAsync(Bom bom);

        Task UpdateAsync(Bom bom);

        Task DeleteAsync(int id);

        Task<bool> ExistsByProductIdAsync(string productId);
        Task<bool> ExistsByMaterialIdAsync(string materialId);
        Task<bool> ExistsAsync(string productId, string materialId, int excludeId);
    }

