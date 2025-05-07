using DBAudit.Infrastructure.Storage;
using Microsoft.AspNetCore.Mvc;

namespace DBAudit.Web.Controllers;

public class ColumnsController(IColumnService columnService) : Controller
{
    [HttpGet("/columns/{tableId:guid}")]
    public IActionResult Index([FromRoute] Guid tableId)
    {
        var columns = columnService.GetByTableId(tableId);

        return View(columns);
    }
}