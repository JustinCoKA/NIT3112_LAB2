using JustinLee_Lab2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JustinLee_Lab2.Data
{
    // Sessions 7 and 9: one context persists QuTech entities and Identity accounts.
    public class QuTechContext : IdentityDbContext<ApplicationUser>
    {
        public QuTechContext(DbContextOptions<QuTechContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<BillingProfile> BillingProfiles { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<PlanModule> PlanModules { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<UsageRecord> UsageRecords { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<AuditEntry> AuditEntries { get; set; }
        public DbSet<Discount> Discounts { get; set; }

        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);
            // TPH keeps inherited contact fields in People with an explicit discriminator.
            model.Entity<Person>().ToTable("People").HasDiscriminator<string>("PersonType")
                .HasValue<Customer>("Customer").HasValue<Administrator>("Administrator");
            model.Entity<BillingProfile>().HasKey(b => b.CustomerId);
            model.Entity<BillingProfile>().HasOne(b => b.Customer).WithOne(c => c.BillingProfile)
                .HasForeignKey<BillingProfile>(b => b.CustomerId).OnDelete(DeleteBehavior.Restrict);
            model.Entity<PlanModule>().HasKey(p => new { p.PlanId, p.ModuleId });
            model.Entity<PlanModule>().HasOne(p => p.Plan).WithMany(p => p.PlanModules)
                .HasForeignKey(p => p.PlanId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            model.Entity<PlanModule>().HasOne(p => p.Module).WithMany(m => m.PlanModules)
                .HasForeignKey(p => p.ModuleId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            model.Entity<Subscription>().HasOne(s => s.Customer).WithMany(c => c.Subscriptions)
                .HasForeignKey(s => s.CustomerId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            model.Entity<Subscription>().HasOne(s => s.Plan).WithMany(p => p.Subscriptions)
                .HasForeignKey(s => s.PlanId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            model.Entity<Subscription>().HasOne(s => s.Discount).WithMany(d => d.Subscriptions)
                .HasForeignKey(s => s.DiscountId).OnDelete(DeleteBehavior.Restrict);
            model.Entity<UsageRecord>().HasOne(u => u.Subscription).WithMany(s => s.UsageRecords)
                .HasForeignKey(u => u.SubscriptionId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            model.Entity<Invoice>().HasOne(i => i.Subscription).WithMany(s => s.Invoices)
                .HasForeignKey(i => i.SubscriptionId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            model.Entity<AuditEntry>().HasOne(a => a.Administrator).WithMany(a => a.AuditEntries)
                .HasForeignKey(a => a.AdministratorId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            model.Entity<ApplicationUser>().HasOne(u => u.Administrator).WithOne()
                .HasForeignKey<ApplicationUser>(u => u.AdministratorId).OnDelete(DeleteBehavior.Restrict);
            // A period can be billed only once, even when two administrators submit together.
            model.Entity<Invoice>().HasIndex(i => new { i.SubscriptionId, i.PeriodStart }).IsUnique();
            model.Entity<Invoice>().HasIndex(i => i.Reference).IsUnique();
            model.Entity<Plan>().HasIndex(p => p.Name).IsUnique();
            model.Entity<Module>().HasIndex(m => m.Name).IsUnique();
            model.Entity<Discount>().HasIndex(d => d.Name).IsUnique();
            SeedData.Configure(model);
        }
    }
}
