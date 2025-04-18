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

        // GET: EnvironmentController
        public ActionResult Index()
        {
            var environments = _environmentService.GetAll();
            return View(environments);
        }

        // GET: EnvironmentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EnvironmentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EnvironmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                collection.TryGetValue("name", out var name);
                collection.TryGetValue("cs", out var connectionString);
                var id = Guid.NewGuid();
                _environmentService.Add(id.ToString(), new Environment
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

        // GET: EnvironmentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EnvironmentController/Activate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Activate(Guid id)
        {
            try
            {
                _environmentService.Activate(id.ToString());
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: EnvironmentController/Deactivate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deactivate(Guid id)
        {
            try
            {
                _environmentService.Deactivate(id.ToString());
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: EnvironmentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EnvironmentController/Delete/5
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