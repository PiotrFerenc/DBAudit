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
    private readonly ITableService _tableService;
    private readonly IEnvironmentService _environmentService;
    private readonly IQueueProvider _queueProvider;
    private readonly IDatabaseService _databaseService;
    private readonly IMetricsService _metricsService;
    private readonly IColumnService _columnService;

    public HomeController(ILogger<HomeController> logger,ITableService tableService, IEnvironmentService environmentService, IQueueProvider queueProvider, IDatabaseService databaseService,  IMetricsService metricsService, IColumnService columnService)
    {
        _logger = logger;
        _tableService = tableService;
        _environmentService = environmentService;
        _queueProvider = queueProvider;
        _databaseService = databaseService;
        _metricsService = metricsService;
        _columnService = columnService;
    }

    public async Task<IActionResult> Index()
    {
        var environments = _environmentService.GetActive();
        if (environments.Count == 0) return View("AddEnv");
        var env = environments.First();
        var databases = _databaseService.GetAll(env.Id);
        ViewBag.Databases = databases;
        ViewBag.EnvironmentId = env.Id;
        var metrics = await _metricsService.GetEnvMetrics(new EnvName(env.Name));
        ViewBag.Metrics = metrics;
        
        return View(env);
    }


    //[HttpGet("/database/{id:guid}")]
    //public IActionResult DatabaseReport([FromRoute] Guid id)
    //{
        //var tables = _tableService.GetAllByDbId(id);
        //ViewBag.Tables = tables;
        //
        //var metrics= _databaseMetricsService.GetAllByDbId(id).Select(x => (x.Title, x.Value.ToString(), "success") );
        //ViewBag.Metrics = metrics.ToList();
        //return View();
    //}
    //[HttpGet("/table/{id:guid}")]
    //public IActionResult TableReport([FromRoute] Guid id)
    //{
        //var columns = _columnService.GetByTableId(id);
        //ViewBag.Columns = columns;
        //
        //var metrics= _tableMetricsService.GetByTableId(id).Select(x => (x.Title, x.Value.ToString(), "success") );
        //ViewBag.Metrics = metrics.ToList();
        //return View();
    //}

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
            return RedirectToAction(nameof(Index));
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
            return RedirectToAction(nameof(Index));
        }
    }
}