using Microsoft.AspNetCore.Mvc;
using Training.ProductApi1.Models;
using Training.ProductApi1.Models.DTOs;
using Training.ProductApi1.Services;
namespace Training.ProductApi1.Controllers;

[ApiController]
[Route("api/materials")]
public class MaterialsController : ControllerBase
{
    private readonly IMaterialService _materialService;
    private readonly ILogger<MaterialsController> _logger;
    public MaterialsController(IMaterialService materialService, ILogger<MaterialsController> logger) 
    {
        _materialService = materialService;
        _logger = logger;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(string? keyword, int pageIndex =1, int pageSize = 10)
    {
        _logger.LogInformation(
        "查詢 Material 列表 Keyword={Keyword}, PageIndex={PageIndex}, PageSize={PageSize}",
        keyword,
        pageIndex,
        pageSize);
        var result = await _materialService
            .GetPagedAsync(keyword, pageIndex, pageSize);


        return Ok(
            ApiResponse<PagedResult<Material>>
            .SuccessResult(
                result,
                "查詢物料列表成功"
            ));
    }
    [HttpPost]
    public async Task<IActionResult> Create(Material material)
    {
        await _materialService.AddAsync(material);
        _logger.LogInformation(
        "新增 Material 成功 MaterialId={MaterialId}",
        material.MaterialId);

        return Ok(
            ApiResponse<Material>.SuccessResult(
                material,
                "新增物料成功"));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Material material)
    {
        if (id != material.MaterialId)
        {
            return BadRequest(
                ApiResponse<object>.FailResult(
                    "Id 不一致", StatusCodes.Status400BadRequest));
        }


        await _materialService.UpdateAsync(material);
        _logger.LogInformation(
        "修改 Material 成功 MaterialId={MaterialId}",
        material.MaterialId);

        return Ok(
             ApiResponse<Material>.SuccessResult(
                 material,
                 "修改物料成功"));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _materialService.DeleteAsync(id);
        _logger.LogInformation(
        "刪除 Material 成功 MaterialId={MaterialId}",
        id);
        return Ok(
            ApiResponse<object>.SuccessResult(
                null,
                "刪除物料成功"));
    }

}
