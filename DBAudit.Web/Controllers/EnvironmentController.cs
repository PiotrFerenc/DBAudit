// Controllers/EnvironmentController.cs

using DBAudit.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

public class EnvironmentController : Controller
{
    private readonly IEnvironmentService _environmentService;

    public EnvironmentController(IEnvironmentService environmentService)
    {
        _environmentService = environmentService;
    }

    // Index (GET: /Environment)
    public async Task<IActionResult> Index()
    {
        var settings = await _environmentService.GetAllAsync();
        return View(settings);
    }
    //
    // // Details (GET: /Environment/Details/1)
    // public IActionResult Details(int id)
    // {
    //     var setting = _environmentService.GetById(id);
    //     if (setting == null) return NotFound();
    //     return View(setting);
    // }

    // Create (GET: /Environment/Create)
    // public IActionResult Create()
    // {
    //     return View();
    // }
    //
    // // Create (POST: /Environment/Create)
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> Create([FromForm] string name, [FromForm] string connectionString)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return View(setting);
    //     }
    //
    //     await _environmentService.CreateAsync(new Name(name), new Name(connectionString));
    //
    //     return RedirectToAction(nameof(Index));
    // }
    //
    //
    //
    // // Edit (POST: /Environment/Edit/1)
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public IActionResult Edit(EnvironmentSetting setting)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return View(setting);
    //     }
    //
    //     _environmentService.Update(setting);
    //     return RedirectToAction(nameof(Index));
    // }
    //
    // // Delete (GET: /Environment/Delete/1)
    // public IActionResult Delete(int id)
    // {
    //     var setting = _environmentService.GetById(id);
    //     if (setting == null) return NotFound();
    //     return View(setting);
    // }
    //
    // // Delete (POST: /Environment/Delete/1)
    // [HttpPost, ActionName("Delete")]
    // [ValidateAntiForgeryToken]
    // public IActionResult DeleteConfirmed(int id)
    // {
    //     _environmentService.Delete(id);
    //     return RedirectToAction(nameof(Index));
    // }
}