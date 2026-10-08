using Microsoft.AspNetCore.Mvc;
using JustinLee_Lab2.Models;

namespace JustinLee_Lab2.Controllers
{
    public class HomeController : Controller
    {
        private readonly DemoData data = new DemoData();
        public IActionResult Index()
        {
            return View(CreateDashboard());
        }
        public IActionResult Analytics()
        {
            return View(CreateDashboard());
        }
        public IActionResult About()
        {
            return View();
        }
        private DashboardViewModel CreateDashboard()
        {
            var model = new DashboardViewModel { CustomerCount = data.Customers.Count, Subscriptions = data.Subscriptions };
            foreach (var subscription in data.Subscriptions)
            {
                if (subscription.Status == "Active")
                {
                    model.ActiveSubscriptions++;
                    model.MonthlyRevenue += subscription.Plan.MonthlyPrice ?? 0;
                }
            }
            foreach (var invoice in data.Invoices)
            {
                if (invoice.Status == "Unpaid")
                {
                    model.UnpaidAmount += invoice.Amount ?? 0;
                    model.OpenInvoices.Add(invoice);
                }
            }
            foreach (var record in data.UsageRecords) model.ApiCalls += record.ApiCalls ?? 0;
            return model;
        }
    }
}
