using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using JustinLee_Lab2.Data;
using JustinLee_Lab2.Models;
namespace JustinLee_Lab2.Controllers
{
    public class HomeController : QuTechAdminController
    {
        public HomeController(QuTechContext db) : base(db) { }
        public IActionResult Index()
        {
            var subscriptions = Db.Subscriptions.Include(x => x.Customer).Include(x => x.Plan).ToList();
            var invoices = Db.Invoices.Include(x => x.Subscription).ThenInclude(x => x.Customer).ToList();
            return View(new DashboardViewModel { CustomerCount = Db.Customers.Count(), ActiveSubscriptions = subscriptions.Count(x => x.Status == "Active"), MonthlyRevenue = subscriptions.Where(x => x.Status == "Active").Sum(x => x.Plan.MonthlyPrice ?? 0), UnpaidAmount = invoices.Where(x => x.Status == "Unpaid").Sum(x => x.Amount ?? 0), Subscriptions = subscriptions, OpenInvoices = invoices.Where(x => x.Status == "Unpaid").ToList() });
        }
        public IActionResult Analytics()
        {
            var usage = Db.UsageRecords.Include(x => x.Subscription).ThenInclude(x => x.Customer).Include(x => x.Subscription.Plan).OrderByDescending(x => x.PeriodStart).ToList();
            ViewBag.TotalCalls = usage.Sum(x => (long)(x.ApiCalls ?? 0));
            var invoices = Db.Invoices.ToList();
            ViewBag.Billed = invoices.Sum(x => x.Amount ?? 0); ViewBag.Paid = invoices.Where(x => x.Status == "Paid").Sum(x => x.Amount ?? 0); ViewBag.Discounts = invoices.Sum(x => x.DiscountAmount);
            return View(usage);
        }
        public IActionResult About() => View();
        [AllowAnonymous]
        public IActionResult Error() { Response.StatusCode = 500; return View(); }
    }
}
