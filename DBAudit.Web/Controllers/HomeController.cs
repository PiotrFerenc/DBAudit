using System.Diagnostics;
using DBAudit.Infrastructure;
using DBAudit.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using DBAudit.Web.Models;

namespace DBAudit.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IEnvironmentService _environmentService;
    private readonly IQueueProvider _queueProvider;

    public HomeController(ILogger<HomeController> logger, IEnvironmentService environmentService, IQueueProvider queueProvider)
    {
        _logger = logger;
        _environmentService = environmentService;
        _queueProvider = queueProvider;
    }

    public IActionResult Index()
    {
        var environments = _environmentService.GetActive();
        // _queueProvider.Enqueue(new EnvironmentMessage(environments.First().Id));

        return View(environments);
    }
}