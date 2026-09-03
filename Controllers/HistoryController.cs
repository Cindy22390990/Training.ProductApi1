using Microsoft.AspNetCore.Mvc;
using Training.ProductApi1.Services;
namespace Training.ProductApi1.Controllers;

[ApiController]
[Route("api/histories")]
public class HistoryController : ControllerBase
{

    private readonly IHistoryService _historyService;


    public HistoryController(
        IHistoryService historyService)
    {
        _historyService = historyService;
    }



    // GET /api/histories
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result =
            await _historyService.GetAllAsync();

        return Ok(result);
    }



    // GET /api/histories/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result =
            await _historyService.GetByIdAsync(id);


        if (result == null)
        {
            return NotFound();
        }


        return Ok(result);
    }
}
