using Microsoft.AspNetCore.Mvc;
using Training.ProductApi1.Models;
using Training.ProductApi1.Services;

namespace Training.ProductApi1.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(
    int pageIndex = 1,
    int pageSize = 10)
    {
        var result = await _productService
            .GetPagedAsync(pageIndex, pageSize);


        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        await _productService.AddAsync(product);

        return Ok(product);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Product product)
    {
        if (id != product.ProductId)
        {
            return BadRequest("Id 不一致");
        }


        await _productService.UpdateAsync(product);


        return Ok(product);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _productService.DeleteAsync(id);

        return Ok();
    }
}
