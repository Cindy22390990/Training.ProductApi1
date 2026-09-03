using Microsoft.AspNetCore.Mvc;
using Training.ProductApi1.Models;
using Training.ProductApi1.Models.DTOs;
using Training.ProductApi1.Services;

namespace Training.ProductApi1.Controllers;

[ApiController]
[Route("api/boms")]
public class BomController : ControllerBase
{
    
    
    private readonly IBomService _bomService;
    public BomController(IBomService bomService)
    {
        _bomService = bomService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(string? keyword, int pageIndex = 1, int pageSize = 10)
    {
        var result = await _bomService
            .GetPagedAsync(keyword, pageIndex, pageSize);


        return Ok(result);
    }
    //POST /api/boms
    [HttpPost]
    public async Task<IActionResult> Create(BomResponseDto dto)
    {
        try
        {

            var bom = new Bom
            {
                ProductId = dto.ProductId,
                MaterialId = dto.MaterialId,
                Quantity = dto.Quantity,
                CreatedAt = DateTime.Now
            };
            await _bomService.AddAsync(bom);

            return Ok(new
            {
                bom.Id,
                bom.ProductId,
                bom.MaterialId,
                bom.Quantity
            });
        }
        catch (Exception exception)
        {
            return BadRequest(new
            {
                message = "新增 BOM 發生錯誤",
                error = exception.Message
            });
        }
    }
        //PUT /api/boms/{id}
        [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, BomUpdateDto dto)
    {
        try
        {
            var bom = await _bomService.GetByIdAsync(id);

            if (bom == null)
                return NotFound();

            bom.MaterialId = dto.MaterialId;
            bom.Quantity = dto.Quantity;

            await _bomService.UpdateAsync(bom);

            return Ok(new
            {
                bom.Id,
                bom.ProductId,
                bom.MaterialId,
                bom.Quantity
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = "修改 BOM 發生錯誤",
                error = ex.Message
            });
        }
    }
    //DELETE /api/boms/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _bomService.DeleteAsync(id);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = "刪除 BOM 發生錯誤",
                error = ex.Message
            });
        }
    }
    //GET /api/boms/by-product 
    [HttpGet("by-product")]
    public async Task<IActionResult> GetByProduct([FromQuery] string productName)
    {
        var result = await _bomService.GetByProductAsync(productName);

        return Ok(result);
    }
    //GET /api/boms/by-material
    [HttpGet("by-material")]
    public async Task<IActionResult> GetByMaterial([FromQuery] string materialName)
    {
        var result = await _bomService.GetByMaterialAsync(materialName);

        return Ok(result);
    }
}
