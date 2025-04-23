using System.Diagnostics;
using DBAudit.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using DBAudit.Web.Models;

namespace DBAudit.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IEnvironmentService _environmentService;

    public HomeController(ILogger<HomeController> logger, IEnvironmentService environmentService)
    {
        _logger = logger;
        _environmentService = environmentService;
    }

    public IActionResult Index()
    {
        var environments = _environmentService.GetActive();
        return View(environments);
    }
}