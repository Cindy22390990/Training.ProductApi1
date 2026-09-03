using Microsoft.EntityFrameworkCore;
using Training.ProductApi1.Models;
using Training.ProductApi1.Models.DTOs;
using Training.ProductApi1.Repositories;

namespace Training.ProductApi1.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IHistoryRepository _historyRepository;
    private readonly IBomRepository _bomRepository;
    public ProductService(IProductRepository productRepository, IHistoryRepository historyRepository,
    IBomRepository bomRepository)
    {
        _productRepository = productRepository;
        _historyRepository = historyRepository;
        _bomRepository = bomRepository;
    }
    

    public async Task<Product?> GetByIdAsync(string id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Product product)
    {
        var exists = await _productRepository.ExistsAsync(product.ProductId);

        if (exists)
        {
            throw new Exception("ProductId 已存在");
        }

        await _productRepository.AddAsync(product);

        var history = new History
        {
            TargetId = product.ProductId,
            Category = "Product",
            Action = "Create",
            Status = "Success"
        };


        await _historyRepository.AddAsync(history);
    }

    public async Task UpdateAsync(Product product)
    {
        var existingProduct = await _productRepository.GetByIdAsync(product.ProductId);


        if (existingProduct == null)
        {
            throw new Exception("Product 不存在");
        }


        existingProduct.Name = product.Name;
        existingProduct.Stock = product.Stock;
        existingProduct.UnitPrice = product.UnitPrice;
        existingProduct.UpdatedAt = DateTime.Now;


        await _productRepository.UpdateAsync(existingProduct);


        var history = new History
        {
            TargetId = product.ProductId,
            Category = "Product",
            Action = "Update",
            Status = "Success"
        };


        await _historyRepository.AddAsync(history);
    }

    public async Task DeleteAsync(string id)
    {
        var product = await _productRepository.GetByIdAsync(id);


        if (product == null)
        {
            throw new Exception("Product 不存在");
        }


        var hasBom = await _bomRepository.ExistsByProductIdAsync(id);


        if (hasBom)
        {
            throw new Exception("此產品已有 BOM 關聯，禁止刪除");
        }


        await _productRepository.DeleteAsync(id);


        var history = new History
        {
            TargetId = id,
            Category = "Product",
            Action = "Delete",
            Status = "Success"
        };


        await _historyRepository.AddAsync(history);
    }
    public async Task<ProductPageResultDto> GetPagedAsync(
    int pageIndex,
    int pageSize)
    {
        var query = _productRepository.GetAll();


        var totalCount = await query.CountAsync();


        var products = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();


        return new ProductPageResultDto
        {
            TotalCount = totalCount,

            TotalPages =
                (int)Math.Ceiling((double)totalCount / pageSize),

            PageIndex = pageIndex,

            PageSize = pageSize,

            Data = products
        };
    }
}
