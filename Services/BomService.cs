using Microsoft.EntityFrameworkCore;
using Training.ProductApi1.Models;
using Training.ProductApi1.Models.DTOs;
using Training.ProductApi1.Repositories;

namespace Training.ProductApi1.Services
{
    public class BomService:IBomService
    {
        private readonly IBomRepository _bomRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly ILogger<BomService> _logger;
        public BomService(IBomRepository bomRepository,IProductRepository productRepository, IMaterialRepository materialRepository, IHistoryRepository historyRepository, ILogger<BomService> logger)
        {
            _bomRepository = bomRepository;
            _productRepository = productRepository;
            _materialRepository = materialRepository;
            _historyRepository = historyRepository;
            _logger = logger;
        }
        public async Task<Bom?> GetByIdAsync(int id)
        {
            return await _bomRepository.GetByIdAsync(id);
        }
        public async Task AddAsync(Bom bom)
        {

            var product = await _productRepository
                .GetByIdAsync(bom.ProductId);

            if (product == null)
            {
                _logger.LogWarning("新增 BOM 失敗，Product不存在 ProductId={ProductId}",bom.ProductId);
                throw new Exception("Product 不存在");
            }
            var material = await _materialRepository
                .GetByIdAsync(bom.MaterialId);
            if (material == null)
            {
                _logger.LogWarning(
                    "新增 BOM 失敗，Material不存在 MaterialId={MaterialId}",
                    bom.MaterialId);
                throw new Exception("Material 不存在");
            }
            var exists = await _bomRepository
                .ExistsAsync(
                   bom.ProductId,
                   bom.MaterialId,
                   0
                );


            if (exists)
            {
                _logger.LogWarning(
                   "新增 BOM 失敗，重複綁定 ProductId={ProductId}, MaterialId={MaterialId}",
                   bom.ProductId,
                   bom.MaterialId);
                throw new Exception(
                    "此產品已綁定此物料"
                );
            }


            await _bomRepository.AddAsync(bom);

            var history = new History
            {
                TargetId = bom.Id.ToString(),
                Category = "Bom",
                Action = "Create",
                Status = "Success"
            };


            await _historyRepository.AddAsync(history);
            _logger.LogInformation(
                "新增 BOM 成功 Id={Id}, ProductId={ProductId}, MaterialId={MaterialId}",
                bom.Id,
                bom.ProductId,
                bom.MaterialId);

        }

        //PUT /api/boms/{id}
        public async Task UpdateAsync(Bom bom)
        {
            var existingBom = await _bomRepository.GetByIdAsync(bom.Id);


            if (existingBom == null)
            {
                _logger.LogWarning(
                    "修改 BOM 失敗，Bom不存在 Id={Id}",
                    bom.Id);

                throw new Exception("Bom 不存在");
            }
            var material = await _materialRepository.GetByIdAsync(bom.MaterialId);


            if (material == null)
            {
                _logger.LogWarning(
                    "修改 BOM 失敗，Material不存在 MaterialId={MaterialId}",
                    bom.MaterialId);

                throw new Exception("Material 不存在");
            }
            var exists =
              await _bomRepository
              .ExistsAsync(
                  existingBom.ProductId,
                  bom.MaterialId,
                  bom.Id
              );


            if (exists)
            {
                _logger.LogWarning(
                    "修改 BOM 失敗，重複綁定 ProductId={ProductId}, MaterialId={MaterialId}",
                    existingBom.ProductId,
                    bom.MaterialId);

                throw new Exception(
                   "此產品已有此物料"
                );
            }
            existingBom.MaterialId = bom.MaterialId;
            existingBom.UpdatedAt = DateTime.Now;


            await _bomRepository.UpdateAsync(existingBom);


            var history = new History
            {
                TargetId = existingBom.Id.ToString(),
                Category = "Bom",
                Action = "Update",
                Status = "Success"
            };


            await _historyRepository.AddAsync(history);
            _logger.LogInformation(
                "修改 BOM 成功 Id={Id}, MaterialId={MaterialId}",
                existingBom.Id,
                existingBom.MaterialId);
        }
        //DELETE /api/boms/{id}
        public async Task DeleteAsync(int id)
        {
            var bom = await _bomRepository.GetByIdAsync(id);


            if (bom == null)
            {
                _logger.LogWarning(
                    "刪除 BOM 失敗，Bom不存在 Id={Id}",
                    id);

                throw new Exception("Bom 不存在");
            }


            await _bomRepository.DeleteAsync(id);
            _logger.LogInformation(
                "刪除 BOM 成功 Id={Id}",
                id);

            var history = new History
            {
                TargetId = id.ToString(),
                Category = "Bom",
                Action = "Delete",
                Status = "Success"
            };


            await _historyRepository.AddAsync(history);
        }

        public async Task<PagedResult<Bom>> GetPagedAsync(
            string? keyword,
            int pageIndex,
            int pageSize)
        {
            _logger.LogInformation(
                "查詢 BOM 分頁 Keyword={Keyword}, PageIndex={PageIndex}, PageSize={PageSize}",
                keyword,
                pageIndex,
                pageSize);
            var query = _bomRepository.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(b =>
                    b.Product.Name.Contains(keyword));
            }
            var totalCount = await query.CountAsync();


            var boms = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();


            return new PagedResult<Bom>
            {
                TotalCount = totalCount,

                TotalPages =
        (int)Math.Ceiling(
            (double)totalCount / pageSize),

                PageIndex = pageIndex,

                PageSize = pageSize,

                Data = boms
            };
        }
        public async Task<List<BomProductResultDto>> GetByProductAsync(string productName)
        {
            _logger.LogInformation(
                "依產品查詢 BOM ProductName={ProductName}",
                productName);
            var query = _bomRepository.GetAll();


            var result = await query
                .Where(b => b.Product.Name.Contains(productName))
                .Select(b => new BomProductResultDto
                {
                    MaterialId = b.MaterialId,

                    MaterialName = b.Material.Name,

                    Quantity = b.Quantity
                })
                .ToListAsync();


            return result;
        }
        public async Task<List<BomMaterialResultDto>> GetByMaterialAsync(string materialName)
        {
            _logger.LogInformation(
                "依物料查詢 BOM MaterialName={MaterialName}",
                materialName);
            var query = _bomRepository.GetAll();


            var result = await query
                .Where(b => b.Material.Name.Contains(materialName))
                .Select(b => new BomMaterialResultDto
                {
                    ProductId = b.ProductId,

                    ProductName = b.Product.Name,

                    Stock = b.Product.Stock,

                    UnitPrice = b.Product.UnitPrice,

                    Quantity = b.Quantity
                })
                .ToListAsync();


            return result;
        }
    }
}
