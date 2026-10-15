# QuTech SaaS administration

NIT3112 Lab Assessment 3 — Justin Lee. Continues the Lab 2 MVC solution with SQLite persistence, ASP.NET Core Identity and an administrator workspace.

## Run the extracted project

Prerequisite: .NET SDK 10.0.401 or a later 10.0.4xx patch. The first restore requires access to NuGet. No database server, connection-string edits or manual migrations are required.

From the folder containing JustinLee-Lab2.sln:

```sh
dotnet run --project JustinLee-Lab2/JustinLee-Lab2.csproj --no-launch-profile --urls http://127.0.0.1:5083
```

Open http://127.0.0.1:5083. On macOS, `bash scripts/run-lab3.sh` applies the file-watcher settings used during verification. On Windows, Visual Studio can open JustinLee-Lab2.sln. The existing solution/project name is retained as permitted continuation of Lab 2; the application is Lab 3.

On first run, EF Core applies the included migration, creates qutech.db in the application content root and seeds fictional business records and administrator accounts. Later starts preserve stored changes. The database file is not shipped in the source ZIP.

Demo administrator: **alex@example.com**

Demo password: **QuTechLab3!2026**

These credentials are exclusively for the local coursework demonstration. Other seeded administrator emails are taylor@example.com, jordan@example.com, casey@example.com and riley@example.com, using the same demonstration password. There is no public registration. Signed-in administrators create further accounts in Administrators. The app uses password hashes, role restrictions, login lockout and antiforgery checks.

## Functional coverage

- Persistent customers and one-to-one billing profiles.
- Identity-linked administrators with account/profile transactions.
- Plans, modules and configurable many-to-many associations.
- Discount validity, percentage and availability; customer subscriptions.
- Monthly API usage and usage/allowance reporting.
- Monthly invoice generation from server-calculated prices and discounts; paid status and print layout.
- Read-only actor/time/target audit entries for business writes and sign-in/sign-out.
- Custom field validation and relationship-aware deletion protection.

Invoices use full monthly fees in AUD, excluding tax, proration and external payment processing. Discounts apply if enabled and valid at the billing-month start. Decimal amounts round to two places away from zero. Monetary invoice values are snapshots; plan names and customer billing addresses are current related-record data. Historical trial/paused seeded invoices represent earlier billing activity; new invoices require an active subscription. Dashboard recurring revenue is active current list prices before discounts. Analytics aggregates all stored records and compares usage with current plan allowances.

No invoice deletion is exposed because accounting history is retained. Audit entries have no edit/delete endpoint. Parent records with dependent history cannot be deleted. A subscription cannot be reassigned to a different customer after billing or usage history exists. Billing profiles are created with a customer and edited through that customer's details.

## Verification

```sh
python3 scripts/lab3_test.py
```

This optional standard-library Python harness builds the app, uses its own test SQLite database and port5086, tests HTTP workflows and database outcomes, restarts the app to verify persistence, and stops its own server. Python is not required to run the application. See [verification](docs/lab3/VERIFIED.md) and [lecture map](docs/SOURCE-MAP.md).

The final report and captures are delivered separately. They are not published in this repository.
