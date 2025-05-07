using DBAudit.Infrastructure.Queue;
using DBAudit.Infrastructure.Storage;
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

        return environments.Any() ? View(environments) : View("AddEnv");
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