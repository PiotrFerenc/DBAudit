using DBAudit.Analyzer.Table;
using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Queue;
using DBAudit.Infrastructure.Storage;
using DBAudit.Infrastructure.Storage.Metrics;
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
    //private readonly IReportService _reportService;
    private readonly IColumnMetricsService _metricsService;

    public HomeController(ILogger<HomeController> logger, IEnvironmentService environmentService, IQueueProvider queueProvider, IDatabaseService databaseService,  IColumnMetricsService metricsService)
    {
        _logger = logger;
        _environmentService = environmentService;
        _queueProvider = queueProvider;
        _databaseService = databaseService;
        _metricsService = metricsService;
    }

    public IActionResult Index()
    {
        var environments = _environmentService.GetActive();
        if (environments.Count == 0) return View("AddEnv");
        var env = environments.First();
        // ViewBag.Count = _metricsService.Count(nameof(IsTableWithoutPrimaryKeys), env.Id);
        // ViewBag.Metrics = _metricsService.Get(nameof(IsTableWithoutPrimaryKeys), env.Id);
        ViewBag.EnvironmentId = env.Id;
        return View(environments);
        //     ViewBag.EnvironmentId = env.Id;
        //     return _reportService.GetByEnvId(env.Id).Match(r => View("Report", r), () => View("Report", new ReportView()));
    }

    [HttpGet("/database/{id:guid}")]
    public IActionResult DatabaseReport([FromRoute] Guid id)
    {
        //return _reportService.GetByDbId(id).Match(r => View("Report", r), () => View("Report", new ReportView()));
        return View();
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