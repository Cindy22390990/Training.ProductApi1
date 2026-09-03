using Microsoft.EntityFrameworkCore;
using Training.ProductApi1.Models;
using Training.ProductApi1.Models.DTOs;
using Training.ProductApi1.Repositories;

namespace Training.ProductApi1.Services;

public class MaterialService : IMaterialService
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IHistoryRepository _historyRepository;
    private readonly IBomRepository _bomRepository;
    public MaterialService(IMaterialRepository materialRepository, IHistoryRepository historyRepository,
    IBomRepository bomRepository)
    {
        _materialRepository = materialRepository;
        _historyRepository = historyRepository;
        _bomRepository = bomRepository;
    }
    public async Task<Material?> GetByIdAsync(string id)
    {
        return await _materialRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Material material)
    {
        var exists = await _materialRepository.ExistsAsync(material.MaterialId);

        if (exists)
        {
            throw new Exception("MaterialId 已存在");
        }

        await _materialRepository.AddAsync(material);

        var history = new History
        {
            TargetId = material.MaterialId,
            Category = "Material",
            Action = "Create",
            Status = "Success"
        };


        await _historyRepository.AddAsync(history);
    }

    public async Task UpdateAsync(Material material)
    {
        var existingMaterial = await _materialRepository.GetByIdAsync(material.MaterialId);


        if (existingMaterial == null)
        {
            throw new Exception("Material 不存在");
        }


        existingMaterial.Name = material.Name;
        existingMaterial.Stock = material.Stock;
        existingMaterial.UpdatedAt = DateTime.Now;


        await _materialRepository.UpdateAsync(existingMaterial);


        var history = new History
        {
            TargetId = material.MaterialId,
            Category = "Material",
            Action = "Update",
            Status = "Success"
        };


        await _historyRepository.AddAsync(history);
    }

    public async Task DeleteAsync(string id)
    {
        var Material = await _materialRepository.GetByIdAsync(id);


        if (Material == null)
        {
            throw new Exception("Material 不存在");
        }


        var hasBom = await _bomRepository.ExistsByMaterialIdAsync(id);


        if (hasBom)
        {
            throw new Exception("此物料已有 BOM 關聯，禁止刪除");
        }


        await _materialRepository.DeleteAsync(id);


        var history = new History
        {
            TargetId = id,
            Category = "Materialt",
            Action = "Delete",
            Status = "Success"
        };


        await _historyRepository.AddAsync(history);
    }
    public async Task<MaterialPageResultDto> GetPagedAsync(
    string? keyword,
    int pageIndex,
    int pageSize)
    {
        var query = _materialRepository.GetAll();
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(m =>
                m.Name.Contains(keyword));
        }

        var totalCount = await query.CountAsync();


        var Material = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();


        return new MaterialPageResultDto
        {
            TotalCount = totalCount,

            TotalPages =
                (int)Math.Ceiling((double)totalCount / pageSize),

            PageIndex = pageIndex,

            PageSize = pageSize,

            Data = Material
        };
    }




}
