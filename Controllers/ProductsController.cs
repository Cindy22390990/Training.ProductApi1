using Microsoft.AspNetCore.Mvc;
using Training.ProductApi1.Models;
using Training.ProductApi1.Services;
using Training.ProductApi1.Models.DTOs;

namespace Training.ProductApi1.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;
    public ProductsController(
        IProductService productService,
        ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(
    int pageIndex = 1,
    int pageSize = 10)
    {
        _logger.LogInformation(
        "查詢 Product 列表 PageIndex={PageIndex}, PageSize={PageSize}",
        pageIndex,
        pageSize);
        var result = await _productService
            .GetPagedAsync(pageIndex, pageSize);


        return Ok(
            ApiResponse<PagedResult<Product>>
            .SuccessResult(
                result,
                "查詢產品列表成功"
    ));
    }
    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        await _productService.AddAsync(product);

        _logger.LogInformation(
            "新增 Product 成功 ProductId={ProductId}",
            product.ProductId);


        return Ok(
            ApiResponse<Product>.SuccessResult(
                product,
                "新增產品成功"));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Product product)
    {
        if (id != product.ProductId)
        {
            return BadRequest(
                ApiResponse<object>.FailResult(
                    "Id 不一致",
                    StatusCodes.Status400BadRequest));
        }


        await _productService.UpdateAsync(product);
        _logger.LogInformation(
        "修改 Product 成功 ProductId={ProductId}",
        product.ProductId);

        return Ok(
            ApiResponse<Product>.SuccessResult(
                product,
                "修改產品成功"));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _productService.DeleteAsync(id);
        _logger.LogInformation(
        "刪除 Product 成功 ProductId={ProductId}",
        id);
        return Ok(
            ApiResponse<object>.SuccessResult(
                null,
                "刪除產品成功"));
    }
}
