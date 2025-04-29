using DBAudit.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Environment = DBAudit.Infrastructure.Data.Entities.Environment;

namespace DBAudit.Web.Controllers
{
    public class EnvironmentController : Controller
    {
        private readonly IEnvironmentService _environmentService;

        public EnvironmentController(IEnvironmentService environmentService)
        {
            _environmentService = environmentService;
        }

        public ActionResult Index()
        {
            var environments = _environmentService.GetAll();
            return View(environments);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
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

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Activate(Guid id)
        {
            try
            {
                _environmentService.Activate(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deactivate(Guid id)
        {
            try
            {
                _environmentService.Deactivate(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}