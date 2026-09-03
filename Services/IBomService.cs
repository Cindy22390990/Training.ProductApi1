using Training.ProductApi1.Models;
using Training.ProductApi1.Models.DTOs;

namespace Training.ProductApi1.Services;

public interface IBomService
{
    Task<BomPageResultDto> GetPagedAsync(
        string? keyword,
        int pageIndex,
        int pageSize);
    Task<Bom?> GetByIdAsync(int id);
    Task AddAsync(Bom bom);
    Task UpdateAsync(Bom bom);
    Task DeleteAsync(int id);
    Task<List<BomProductResultDto>> GetByProductAsync(string productName);
    Task<List<BomMaterialResultDto>> GetByMaterialAsync(string materialName);
}
