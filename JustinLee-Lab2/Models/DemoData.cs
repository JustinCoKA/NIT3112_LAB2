namespace JustinLee_Lab2.Models
{
    // Session 3: object/collection initialisers. Each request gets fresh fictional data.
    // Nothing is written to a database and form previews do not change these lists.
    public class DemoData
    {
        public List<Customer> Customers { get; set; } = new List<Customer>
        {
            new Customer { Id = 1, FullName = "Mia Chen", CompanyName = "Harbour Studio", Email = "mia@example.com", Phone = "+61 2 5550 0101" },
            new Customer { Id = 2, FullName = "Oliver Patel", CompanyName = "Summit Logistics", Email = "oliver@example.com", Phone = "+61 3 5550 0102" },
            new Customer { Id = 3, FullName = "Amelia Brooks", CompanyName = "Greenfield Retail", Email = "amelia@example.com", Phone = "+61 7 5550 0103" }
        };
        public List<Administrator> Administrators { get; set; } = new List<Administrator>
        {
            new Administrator { Id = 101, FullName = "Alex Morgan", Email = "alex@example.com", Phone = "+61 2 5550 0201", Department = "Platform operations" },
            new Administrator { Id = 102, FullName = "Taylor Kim", Email = "taylor@example.com", Phone = "+61 2 5550 0202", Department = "Billing support" }
        };
        public List<BillingProfile> BillingProfiles { get; set; } = new List<BillingProfile>
        {
            new BillingProfile { CustomerId = 1, BillingName = "Harbour Studio", BillingEmail = "accounts.harbour@example.com", Address = "10 Example Street, Sydney NSW 2000" },
            new BillingProfile { CustomerId = 2, BillingName = "Summit Logistics", BillingEmail = "accounts.summit@example.com", Address = "20 Example Street, Melbourne VIC 3000" },
            new BillingProfile { CustomerId = 3, BillingName = "Greenfield Retail", BillingEmail = "accounts.greenfield@example.com", Address = "30 Example Street, Brisbane QLD 4000" }
        };
        public List<Plan> Plans { get; set; } = new List<Plan>
        {
            new Plan { Id = 1, Name = "Starter", Description = "Essential tools for a small team launching its first workspace.", MonthlyPrice = 49m, IncludedApiCalls = 10000 },
            new Plan { Id = 2, Name = "Growth", Description = "Billing and analytics for businesses with growing operations.", MonthlyPrice = 149m, IncludedApiCalls = 50000 },
            new Plan { Id = 3, Name = "Scale", Description = "The complete module catalogue and a larger API allowance.", MonthlyPrice = 399m, IncludedApiCalls = 200000 }
        };
        public List<Module> Modules { get; set; } = new List<Module>
        {
            new Module { Id = 1, Name = "Workspace", Description = "Manage service settings and customer workspaces." },
            new Module { Id = 2, Name = "Billing", Description = "Review subscription charges and invoice records." },
            new Module { Id = 3, Name = "Analytics", Description = "Understand usage and subscription performance." },
            new Module { Id = 4, Name = "Audit", Description = "Review administrator actions and affected records." }
        };
        public List<PlanModule> PlanModules { get; set; } = new List<PlanModule>
        {
            new PlanModule { PlanId = 1, ModuleId = 1, Enabled = true },
            new PlanModule { PlanId = 2, ModuleId = 1, Enabled = true },
            new PlanModule { PlanId = 2, ModuleId = 2, Enabled = true },
            new PlanModule { PlanId = 2, ModuleId = 3, Enabled = true },
            new PlanModule { PlanId = 3, ModuleId = 1, Enabled = true },
            new PlanModule { PlanId = 3, ModuleId = 2, Enabled = true },
            new PlanModule { PlanId = 3, ModuleId = 3, Enabled = true },
            new PlanModule { PlanId = 3, ModuleId = 4, Enabled = true }
        };
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>
        {
            new Subscription { Id = 1, CustomerId = 1, PlanId = 2, StartDate = new DateTime(2026, 8, 1), Status = "Active" },
            new Subscription { Id = 2, CustomerId = 2, PlanId = 3, StartDate = new DateTime(2026, 7, 1), Status = "Active" },
            new Subscription { Id = 3, CustomerId = 3, PlanId = 1, StartDate = new DateTime(2026, 9, 1), Status = "Trial" }
        };
        public List<UsageRecord> UsageRecords { get; set; } = new List<UsageRecord>
        {
            new UsageRecord { Id = 1, SubscriptionId = 1, PeriodStart = new DateTime(2026, 9, 1), ApiCalls = 32400 },
            new UsageRecord { Id = 2, SubscriptionId = 2, PeriodStart = new DateTime(2026, 9, 1), ApiCalls = 142600 },
            new UsageRecord { Id = 3, SubscriptionId = 3, PeriodStart = new DateTime(2026, 9, 1), ApiCalls = 7200 }
        };
        public List<Invoice> Invoices { get; set; } = new List<Invoice>
        {
            new Invoice { Id = 1, Reference = "QT-2026-0901", SubscriptionId = 1, IssuedOn = new DateTime(2026, 9, 1), DueOn = new DateTime(2026, 9, 15), Amount = 149m, Status = "Paid" },
            new Invoice { Id = 2, Reference = "QT-2026-0902", SubscriptionId = 2, IssuedOn = new DateTime(2026, 9, 1), DueOn = new DateTime(2026, 9, 15), Amount = 399m, Status = "Unpaid" }
        };
        public List<AuditEntry> AuditEntries { get; set; } = new List<AuditEntry>
        {
            new AuditEntry { Id = 1, AdministratorId = 101, OccurredAt = new DateTime(2026, 9, 1, 9, 15, 0), Action = "Assigned Growth plan to Harbour Studio", Target = "Subscription #1" },
            new AuditEntry { Id = 2, AdministratorId = 102, OccurredAt = new DateTime(2026, 9, 1, 10, 30, 0), Action = "Prepared the monthly Summit Logistics invoice", Target = "Invoice QT-2026-0902" }
        };
        public DemoData()
        {
            // Link both ends so the sample graph represents the ERD relationships.
            foreach (var profile in BillingProfiles)
            {
                profile.Customer = Customers.Find(c => c.Id == profile.CustomerId);
                profile.Customer.BillingProfile = profile;
            }
            foreach (var link in PlanModules)
            {
                link.Plan = Plans.Find(p => p.Id == link.PlanId);
                link.Module = Modules.Find(m => m.Id == link.ModuleId);
                link.Plan.PlanModules.Add(link);
                link.Module.PlanModules.Add(link);
            }
            foreach (var subscription in Subscriptions)
            {
                subscription.Customer = Customers.Find(c => c.Id == subscription.CustomerId);
                subscription.Plan = Plans.Find(p => p.Id == subscription.PlanId);
                subscription.Customer.Subscriptions.Add(subscription);
                subscription.Plan.Subscriptions.Add(subscription);
            }
            foreach (var record in UsageRecords)
            {
                record.Subscription = Subscriptions.Find(s => s.Id == record.SubscriptionId);
                record.Subscription.UsageRecords.Add(record);
            }
            foreach (var invoice in Invoices)
            {
                invoice.Subscription = Subscriptions.Find(s => s.Id == invoice.SubscriptionId);
                invoice.Subscription.Invoices.Add(invoice);
            }
            foreach (var entry in AuditEntries)
            {
                entry.Administrator = Administrators.Find(a => a.Id == entry.AdministratorId);
                entry.Administrator.AuditEntries.Add(entry);
            }
        }
    }
}
