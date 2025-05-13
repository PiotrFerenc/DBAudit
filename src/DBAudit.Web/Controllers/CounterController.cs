using DBAudit.Infrastructure.Storage;
using Microsoft.AspNetCore.Mvc;

namespace DBAudit.Web.Controllers;

public class CounterController(ICounterService counterService) : Controller
{
    // GET
    [HttpGet("/counter/{id:guid}")]
    public IActionResult Index([FromRoute] Guid id)
        => counterService.Get(id).Match(View, () => View("Alert/Error", ("No counter found for this report", "")));
}