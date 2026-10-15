using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JustinLee_Lab2.Migrations
{
    /// <inheritdoc />
    public partial class InitialQuTech : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    Percentage = table.Column<decimal>(type: "TEXT", nullable: false),
                    StartsOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndsOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    PersonType = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    Department = table.Column<string>(type: "TEXT", maxLength: 60, nullable: true),
                    CompanyName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    MonthlyPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    IncludedApiCalls = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    AdministratorId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_People_AdministratorId",
                        column: x => x.AdministratorId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AdministratorId = table.Column<int>(type: "INTEGER", nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Action = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Target = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditEntries_People_AdministratorId",
                        column: x => x.AdministratorId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BillingProfiles",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    BillingName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    BillingEmail = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingProfiles", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_BillingProfiles_People_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanModules",
                columns: table => new
                {
                    PlanId = table.Column<int>(type: "INTEGER", nullable: false),
                    ModuleId = table.Column<int>(type: "INTEGER", nullable: false),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanModules", x => new { x.PlanId, x.ModuleId });
                    table.ForeignKey(
                        name: "FK_PlanModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanModules_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlanId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    DiscountId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscriptions_People_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Reference = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    SubscriptionId = table.Column<int>(type: "INTEGER", nullable: false),
                    IssuedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DueOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Subtotal = table.Column<decimal>(type: "TEXT", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsageRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SubscriptionId = table.Column<int>(type: "INTEGER", nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ApiCalls = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsageRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsageRecords_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "Id", "Description", "Enabled", "EndsOn", "Name", "Percentage", "StartsOn" },
                values: new object[,]
                {
                    { 1, "Welcome programme: 10% off eligible monthly subscription charges.", true, new DateTime(2027, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Welcome", 10m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Loyalty programme: 15% off eligible monthly subscription charges.", true, new DateTime(2027, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Loyalty", 15m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Education programme: 20% off eligible monthly subscription charges.", true, new DateTime(2027, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Education", 20m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Community programme: 25% off eligible monthly subscription charges.", true, new DateTime(2027, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Community", 25m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Seasonal programme: 5% off eligible monthly subscription charges.", true, new DateTime(2027, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Seasonal", 5m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Manage customer workspaces and service settings.", "Workspace" },
                    { 2, "Generate invoices and track payment status.", "Billing" },
                    { 3, "Review subscription revenue and API usage.", "Analytics" },
                    { 4, "Review accountable administrator activity.", "Audit" },
                    { 5, "Connect business applications through API access.", "Integrations" }
                });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "CompanyName", "Email", "FullName", "PersonType", "Phone" },
                values: new object[,]
                {
                    { 1, "Harbour Studio", "mia@example.com", "Mia Chen", "Customer", "+61 2 5550 0101" },
                    { 2, "Summit Logistics", "oliver@example.com", "Oliver Patel", "Customer", "+61 2 5550 0102" },
                    { 3, "Greenfield Retail", "amelia@example.com", "Amelia Brooks", "Customer", "+61 2 5550 0103" },
                    { 4, "Coastal Design", "noah@example.com", "Noah Wilson", "Customer", "+61 2 5550 0104" },
                    { 5, "Riverbank Consulting", "ava@example.com", "Ava Singh", "Customer", "+61 2 5550 0105" }
                });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "Department", "Email", "FullName", "PersonType", "Phone" },
                values: new object[,]
                {
                    { 101, "Platform operations", "alex@example.com", "Alex Morgan", "Administrator", "+61 2 5550 0201" },
                    { 102, "Billing support", "taylor@example.com", "Taylor Kim", "Administrator", "+61 2 5550 0202" },
                    { 103, "Customer support", "jordan@example.com", "Jordan Ellis", "Administrator", "+61 2 5550 0203" },
                    { 104, "Service configuration", "casey@example.com", "Casey Tran", "Administrator", "+61 2 5550 0204" },
                    { 105, "Compliance", "riley@example.com", "Riley Park", "Administrator", "+61 2 5550 0205" }
                });

            migrationBuilder.InsertData(
                table: "Plans",
                columns: new[] { "Id", "Description", "IncludedApiCalls", "MonthlyPrice", "Name" },
                values: new object[,]
                {
                    { 1, "Starter subscription for QuTech customer workspaces with 10000 monthly API calls.", 10000, 49m, "Starter" },
                    { 2, "Growth subscription for QuTech customer workspaces with 50000 monthly API calls.", 50000, 149m, "Growth" },
                    { 3, "Scale subscription for QuTech customer workspaces with 200000 monthly API calls.", 200000, 399m, "Scale" },
                    { 4, "Enterprise subscription for QuTech customer workspaces with 500000 monthly API calls.", 500000, 799m, "Enterprise" },
                    { 5, "Community subscription for QuTech customer workspaces with 5000 monthly API calls.", 5000, 29m, "Community" }
                });

            migrationBuilder.InsertData(
                table: "AuditEntries",
                columns: new[] { "Id", "Action", "AdministratorId", "OccurredAt", "Target" },
                values: new object[,]
                {
                    { 1, "Seeded demonstration subscription and October invoice", 101, new DateTime(2026, 10, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), "Subscription #1" },
                    { 2, "Seeded demonstration subscription and October invoice", 102, new DateTime(2026, 10, 1, 9, 5, 0, 0, DateTimeKind.Unspecified), "Subscription #2" },
                    { 3, "Seeded demonstration subscription and October invoice", 103, new DateTime(2026, 10, 1, 9, 10, 0, 0, DateTimeKind.Unspecified), "Subscription #3" },
                    { 4, "Seeded demonstration subscription and October invoice", 104, new DateTime(2026, 10, 1, 9, 15, 0, 0, DateTimeKind.Unspecified), "Subscription #4" },
                    { 5, "Seeded demonstration subscription and October invoice", 105, new DateTime(2026, 10, 1, 9, 20, 0, 0, DateTimeKind.Unspecified), "Subscription #5" }
                });

            migrationBuilder.InsertData(
                table: "BillingProfiles",
                columns: new[] { "CustomerId", "Address", "BillingEmail", "BillingName" },
                values: new object[,]
                {
                    { 1, "10 Example Street, Sydney NSW 2000", "accounts.mia@example.com", "Harbour Studio" },
                    { 2, "20 Example Street, Sydney NSW 2000", "accounts.oliver@example.com", "Summit Logistics" },
                    { 3, "30 Example Street, Sydney NSW 2000", "accounts.amelia@example.com", "Greenfield Retail" },
                    { 4, "40 Example Street, Sydney NSW 2000", "accounts.noah@example.com", "Coastal Design" },
                    { 5, "50 Example Street, Sydney NSW 2000", "accounts.ava@example.com", "Riverbank Consulting" }
                });

            migrationBuilder.InsertData(
                table: "PlanModules",
                columns: new[] { "ModuleId", "PlanId", "Enabled" },
                values: new object[,]
                {
                    { 1, 1, true },
                    { 2, 1, true },
                    { 1, 2, true },
                    { 2, 2, true },
                    { 3, 2, true },
                    { 1, 3, true },
                    { 2, 3, true },
                    { 3, 3, true },
                    { 4, 3, true },
                    { 1, 4, true },
                    { 2, 4, true },
                    { 3, 4, true },
                    { 4, 4, true },
                    { 5, 4, true },
                    { 1, 5, true }
                });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "Id", "CustomerId", "DiscountId", "PlanId", "StartDate", "Status" },
                values: new object[,]
                {
                    { 1, 1, 1, 2, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { 2, 2, 2, 3, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { 3, 3, 3, 1, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trial" },
                    { 4, 4, 4, 4, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { 5, 5, 5, 5, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Paused" }
                });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "Id", "Amount", "DiscountAmount", "DueOn", "IssuedOn", "PeriodStart", "Reference", "Status", "SubscriptionId", "Subtotal" },
                values: new object[,]
                {
                    { 1, 134.1m, 14.9m, new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "QT-2026-10-001", "Paid", 1, 149m },
                    { 2, 339.15m, 59.85m, new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "QT-2026-10-002", "Unpaid", 2, 399m },
                    { 3, 39.2m, 9.8m, new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "QT-2026-10-003", "Paid", 3, 49m },
                    { 4, 599.25m, 199.75m, new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "QT-2026-10-004", "Unpaid", 4, 799m },
                    { 5, 27.55m, 1.45m, new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "QT-2026-10-005", "Paid", 5, 29m }
                });

            migrationBuilder.InsertData(
                table: "UsageRecords",
                columns: new[] { "Id", "ApiCalls", "PeriodStart", "SubscriptionId" },
                values: new object[,]
                {
                    { 1, 32400, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 142600, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 3, 7200, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 4, 185000, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 5, 2100, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AdministratorId",
                table: "AspNetUsers",
                column: "AdministratorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntries_AdministratorId",
                table: "AuditEntries",
                column: "AdministratorId");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_Name",
                table: "Discounts",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_Reference",
                table: "Invoices",
                column: "Reference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SubscriptionId_PeriodStart",
                table: "Invoices",
                columns: new[] { "SubscriptionId", "PeriodStart" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_Name",
                table: "Modules",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanModules_ModuleId",
                table: "PlanModules",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_Name",
                table: "Plans",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_CustomerId",
                table: "Subscriptions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_DiscountId",
                table: "Subscriptions",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PlanId",
                table: "Subscriptions",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_UsageRecords_SubscriptionId",
                table: "UsageRecords",
                column: "SubscriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuditEntries");

            migrationBuilder.DropTable(
                name: "BillingProfiles");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "PlanModules");

            migrationBuilder.DropTable(
                name: "UsageRecords");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Plans");
        }
    }
}
