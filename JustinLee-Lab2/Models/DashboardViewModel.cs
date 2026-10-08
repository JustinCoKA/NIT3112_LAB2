namespace JustinLee_Lab2.Models
{
    // Presentation values are calculated by the controller, not the Razor view.
    public class DashboardViewModel
    {
        public int CustomerCount { get; set; }
        public int ActiveSubscriptions { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public decimal UnpaidAmount { get; set; }
        public int ApiCalls { get; set; }
        public List<Invoice> OpenInvoices { get; set; } = new List<Invoice>();
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
