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
    private readonly IDatabaseService _databaseService;
    private readonly IReportService _reportService;

    public HomeController(ILogger<HomeController> logger, IEnvironmentService environmentService, IQueueProvider queueProvider, IDatabaseService databaseService, IReportService reportService)
    {
        _logger = logger;
        _environmentService = environmentService;
        _queueProvider = queueProvider;
        _databaseService = databaseService;
        _reportService = reportService;
    }

    public IActionResult Index()
    {
        var environments = _environmentService.GetActive();
        if (environments.Count == 0) return View("AddEnv");
        var env = environments.First();
        ViewBag.EnvironmentId = env.Id;
        return _reportService.GetByEnvId(env.Id).Match(r => View("Report", r), () => View("Report", new ReportView()));
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult AnalyzeDatabase(IFormCollection collection)
    {
        try
        {
            if (collection.TryGetValue("envId", out var name) && Guid.TryParse(name, out var envId))
            {
                _queueProvider.Enqueue(new DatabaseAnalyzerMessage(envId));
            }

            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}