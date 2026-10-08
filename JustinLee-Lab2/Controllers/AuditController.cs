using Microsoft.AspNetCore.Mvc;
using JustinLee_Lab2.Models;
namespace JustinLee_Lab2.Controllers
{
    public class AuditController : Controller
    {
        public IActionResult Index()
        {
            // Audit records are read-only; real automatic capture belongs to Lab 3.
            return View(new DemoData().AuditEntries);
        }
    }
}
