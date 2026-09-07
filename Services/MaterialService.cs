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
    private readonly ILogger<MaterialService> _logger;
    public MaterialService(IMaterialRepository materialRepository, IHistoryRepository historyRepository,
    IBomRepository bomRepository, ILogger<MaterialService> logger)
    {
        _materialRepository = materialRepository;
        _historyRepository = historyRepository;
        _bomRepository = bomRepository;
        _logger = logger;
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
            _logger.LogWarning(
                "新增 Material 失敗，MaterialId 已存在 Id={MaterialId}",
                material.MaterialId);

            throw new Exception("MaterialId 已存在");
        }

        await _materialRepository.AddAsync(material);
        _logger.LogInformation(
            "新增 Material 成功 MaterialId={MaterialId}",
            material.MaterialId);
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
            _logger.LogWarning(
                "修改 Material 失敗，不存在 MaterialId={MaterialId}",
                material.MaterialId);

            throw new Exception("Material 不存在");
        }


        existingMaterial.Name = material.Name;
        existingMaterial.Stock = material.Stock;
        existingMaterial.UpdatedAt = DateTime.Now;


        await _materialRepository.UpdateAsync(existingMaterial);
        _logger.LogInformation(
            "修改 Material 成功 MaterialId={MaterialId}",
            material.MaterialId);

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
        var material = await _materialRepository.GetByIdAsync(id);


        if (material == null)
        {
            _logger.LogWarning(
                "刪除 Material 失敗，不存在 MaterialId={MaterialId}",
                id);

            throw new Exception("Material 不存在");
        }


        var hasBom = await _bomRepository.ExistsByMaterialIdAsync(id);


        if (hasBom)
        {
            _logger.LogWarning(
                "刪除 Material 失敗，存在 BOM 關聯 MaterialId={MaterialId}",
                id);

            throw new Exception(
                "此物料已有 BOM 關聯，禁止刪除");
        }


        await _materialRepository.DeleteAsync(id);
        _logger.LogInformation(
            "刪除 Material 成功 MaterialId={MaterialId}",
            id);

        var history = new History
        {
            TargetId = id,
            Category = "Material",
            Action = "Delete",
            Status = "Success"
        };


        await _historyRepository.AddAsync(history);
    }
    public async Task<PagedResult<Material>> GetPagedAsync(
    string? keyword,
    int pageIndex,
    int pageSize)
    {
        if (pageIndex <= 0)
        {
            pageIndex = 1;
        }

        if (pageSize <= 0)
        {
            pageSize = 10;
        }
        var query = _materialRepository.GetAll();
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(m =>
                m.Name.Contains(keyword));
        }

        var totalCount = await query.CountAsync();


        var materials = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();


        return new PagedResult<Material>
        {
            TotalCount = totalCount,

            TotalPages =
                (int)Math.Ceiling((double)totalCount / pageSize),

            PageIndex = pageIndex,

            PageSize = pageSize,

            Data = materials
        };
    }




}
