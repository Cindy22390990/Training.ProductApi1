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
        public BomService(IBomRepository bomRepository,IProductRepository productRepository, IMaterialRepository materialRepository, IHistoryRepository historyRepository)
        {
            _bomRepository = bomRepository;
            _productRepository = productRepository;
            _materialRepository = materialRepository;
            _historyRepository = historyRepository;
            
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
                throw new Exception("Product 不存在");
            }
            var material = await _materialRepository
                .GetByIdAsync(bom.MaterialId);
            if (material == null)
            {
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

        }

        //PUT /api/boms/{id}
        public async Task UpdateAsync(Bom bom)
        {
            var existingBom = await _bomRepository.GetByIdAsync(bom.Id);


            if (existingBom == null)
            {
                throw new Exception("Bom 不存在");
            }
            var material = await _materialRepository.GetByIdAsync(bom.MaterialId);


            if (material == null)
            {
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
        }
        //DELETE /api/boms/{id}
        public async Task DeleteAsync(int id)
        {
            var bom = await _bomRepository.GetByIdAsync(id);


            if (bom == null)
            {
                throw new Exception("Bom 不存在");
            }


            await _bomRepository.DeleteAsync(id);


            var history = new History
            {
                TargetId = id.ToString(),
                Category = "Bom",
                Action = "Delete",
                Status = "Success"
            };


            await _historyRepository.AddAsync(history);
        }

        public async Task<BomPageResultDto> GetPagedAsync(
            string? keyword,
            int pageIndex,
            int pageSize)
        {
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


            return new BomPageResultDto
            {
                TotalCount = totalCount,

                TotalPages =
                    (int)Math.Ceiling((double)totalCount / pageSize),

                PageIndex = pageIndex,

                PageSize = pageSize,

                Data = boms
            };
        }
        public async Task<List<BomProductResultDto>> GetByProductAsync(string productName)
        {
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
