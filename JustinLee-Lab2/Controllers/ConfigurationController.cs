using Microsoft.AspNetCore.Mvc;
using JustinLee_Lab2.Models;
namespace JustinLee_Lab2.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly DemoData data = new DemoData();
        public IActionResult Index() { return View(data.PlanModules); }
        public IActionResult Create()
        {
            ViewBag.Plans = data.Plans;
            ViewBag.Modules = data.Modules;
            return View(new PlanModule { Enabled = true });
        }
        [HttpPost]
        public IActionResult Create(PlanModule model)
        {
            if (data.Plans.Find(p => p.Id == model.PlanId) == null)
            {
                ModelState.AddModelError("PlanId", "Select an existing subscription plan.");
            }
            if (data.Modules.Find(m => m.Id == model.ModuleId) == null)
            {
                ModelState.AddModelError("ModuleId", "Select an existing module.");
            }
            ViewBag.Plans = data.Plans;
            ViewBag.Modules = data.Modules;
            if (!ModelState.IsValid) return View(model);
            ViewBag.IsPreview = true;
            return View(model);
        }
    }
}
