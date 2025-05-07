using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Queue;
using DBAudit.Infrastructure.Storage;
using DBAudit.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Environment = DBAudit.Infrastructure.Contracts.Entities.Environment;

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
        if (environments.Count == 0) return View("AddEnv");

        var report = new ReportView
        {
            Title = "test",
            Links =
            [
                ("Database Overview", "/database/overview"),
                ("Security Audit", "/audit/security"),
                ("Performance Metrics", "/metrics/performance"),
                ("User Activities", "/audit/users")
            ]
        };
        return View("Report", report);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
    {
        try
        {
            collection.TryGetValue("name", out var name);
            collection.TryGetValue("cs", out var connectionString);
            var id = Guid.NewGuid();
            _environmentService.Add(new Environment
            {
                Id = id,
                Name = name,
                IsActive = true,
                ConnectionString = connectionString
            });
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}