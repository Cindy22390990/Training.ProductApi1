using Microsoft.AspNetCore.Mvc;
using Training.ProductApi1.Models;
using Training.ProductApi1.Services;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
namespace Training.ProductApi1.Controllers;

[ApiController]
[Route("api/materials")]
public class MaterialsController : ControllerBase
{
    private readonly IMaterialService _materialService;
    public MaterialsController(IMaterialService materialService) 
    {
        _materialService = materialService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(string? keyword, int pageIndex =1, int pageSize = 10)
    {
        var result = await _materialService
            .GetPagedAsync(keyword, pageIndex, pageSize);


        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> Create(Material material)
    {
        await _materialService.AddAsync(material);

        return Ok(material);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Material material)
    {
        if (id != material.MaterialId)
        {
            return BadRequest("Id 不一致");
        }


        await _materialService.UpdateAsync(material);


        return Ok(material);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _materialService.DeleteAsync(id);

        return Ok();
    }

}
