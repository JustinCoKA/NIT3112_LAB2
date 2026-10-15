using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JustinLee_Lab2.Data;
using JustinLee_Lab2.Models;
namespace JustinLee_Lab2.Controllers
{
    public class ConfigurationController : QuTechAdminController
    {
        public ConfigurationController(QuTechContext db) : base(db) { }
        public IActionResult Index() => View(Db.PlanModules.Include(x => x.Plan).Include(x => x.Module).ToList());
        public IActionResult Create() { Choices(); return View(new PlanModule { Enabled = true }); }
        [HttpPost]
        public IActionResult Create([Bind("PlanId,ModuleId,Enabled")] PlanModule model)
        {
            if (!Db.Plans.Any(x => x.Id == model.PlanId)) ModelState.AddModelError("PlanId", "Choose an existing plan.");
            if (!Db.Modules.Any(x => x.Id == model.ModuleId)) ModelState.AddModelError("ModuleId", "Choose an existing module.");
            if (!ModelState.IsValid) { Choices(); return View(model); }
            var existing = Db.PlanModules.Find(model.PlanId, model.ModuleId);
            if (existing == null) Db.PlanModules.Add(model); else existing.Enabled = model.Enabled;
            if (!SaveAudited("Configured plan module", "Plan #" + model.PlanId + " / Module #" + model.ModuleId)) { Choices(); return View(model); }
            TempData["Success"] = "Module availability saved."; return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Delete(int planId, int moduleId)
        {
            var model = Db.PlanModules.Find(planId, moduleId); if (model == null) return NotFound();
            Db.PlanModules.Remove(model);
            if (!SaveAudited("Removed plan module", "Plan #" + planId + " / Module #" + moduleId)) { TempData["Error"] = "The assignment could not be removed."; }
            else TempData["Success"] = "Module assignment removed.";
            return RedirectToAction(nameof(Index));
        }
        private void Choices() { ViewBag.Plans = Db.Plans.ToList(); ViewBag.Modules = Db.Modules.ToList(); }
    }
}
