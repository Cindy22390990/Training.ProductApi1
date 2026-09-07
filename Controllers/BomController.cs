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
    private readonly ILogger<BomController> _logger;
    public BomController(
    IBomService bomService,
    ILogger<BomController> logger)
    {
        _bomService = bomService;
        _logger = logger;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(string? keyword, int pageIndex = 1, int pageSize = 10)
    {
        _logger.LogInformation("查詢 BOM 列表 keyword={Keyword}, pageIndex={PageIndex}, pageSize={PageSize}",
            keyword,
            pageIndex,
            pageSize);
        var result = await _bomService
            .GetPagedAsync(keyword, pageIndex, pageSize);


        return Ok(
            ApiResponse<PagedResult<Bom>>
            .SuccessResult(
                result,
                "查詢 BOM 列表成功"));
    }
    //POST /api/boms
    [HttpPost]
    public async Task<IActionResult> Create(BomCreateDto dto)
    {
        

            var bom = new Bom
            {
                ProductId = dto.ProductId,
                MaterialId = dto.MaterialId,
                Quantity = dto.Quantity,
                CreatedAt = DateTime.Now
            };
            await _bomService.AddAsync(bom);
            _logger.LogInformation("新增成功 ProductId={ProductId} MaterialId={MaterialId}", dto.ProductId, dto.MaterialId);

        var result = new BomResultDto
        {
            Id = bom.Id,
            ProductId = bom.ProductId,
            MaterialId = bom.MaterialId,
            Quantity = bom.Quantity
        };


        return Ok(
            ApiResponse<BomResultDto>
            .SuccessResult(
                result,
                "新增 BOM 成功"));



    }
        //PUT /api/boms/{id}
        [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, BomUpdateDto dto)
    {

            var bom = await _bomService.GetByIdAsync(id);

        if (bom == null)
        {
            _logger.LogWarning(
                "修改 BOM 找不到 Id={Id}",
                id);


            return NotFound(
                ApiResponse<object>.FailResult(
                    "Bom 不存在"));
        }

        bom.MaterialId = dto.MaterialId;
            bom.Quantity = dto.Quantity;

            await _bomService.UpdateAsync(bom);

            _logger.LogInformation("修改 BOM 成功 Id={Id}",bom.Id);
        var result = new BomResultDto
        {
            Id = bom.Id,
            ProductId = bom.ProductId,
            MaterialId = bom.MaterialId,
            Quantity = bom.Quantity
        };


        return Ok(
            ApiResponse<BomResultDto>
            .SuccessResult(
                result,
                "修改 BOM 成功"));


    }
    //DELETE /api/boms/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
       
            await _bomService.DeleteAsync(id);
            _logger.LogInformation("刪除 BOM 成功 Id={Id}",id);

            return Ok(
                ApiResponse<bool>
                .SuccessResult(
                    true,
                    "刪除 BOM 成功"));

    }
    //GET /api/boms/by-product 
    [HttpGet("by-product")]
    public async Task<IActionResult> GetByProduct([FromQuery] string productName)
    {
        var result = await _bomService.GetByProductAsync(productName);

        return Ok(
        ApiResponse<List<BomProductResultDto>>
        .SuccessResult(
            result,
            "查詢產品 BOM 成功"));
    }
    //GET /api/boms/by-material
    [HttpGet("by-material")]
    public async Task<IActionResult> GetByMaterial([FromQuery] string materialName)
    {
        var result = await _bomService.GetByMaterialAsync(materialName);

        return Ok(
        ApiResponse<List<BomMaterialResultDto>>
        .SuccessResult(
            result,
            "查詢物料 BOM 成功"));
    }
}
