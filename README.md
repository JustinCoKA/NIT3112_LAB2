# JustinLee-Lab2 · QuTech SaaS administration

NIT3112 Lab Assessment 2 — Justin Lee

A running ASP.NET Core MVC prototype with C# models, Razor views and fictional business data. The design follows the actual QuTech brief and the supplied Week 1–2 lecture material. [Teaching-source map](docs/SOURCE-MAP.md).

## Run

Requires the .NET 10 SDK (the version introduced in Session 1 and available on the development machine).

```sh
dotnet build JustinLee-Lab2.sln
dotnet run --project JustinLee-Lab2/JustinLee-Lab2.csproj
```

Open http://localhost:5080. In Visual Studio, open `JustinLee-Lab2.sln`, select the JustinLee-Lab2 profile and run.

In the restricted macOS development environment, the default configuration file watcher stalled during startup. This per-process workaround was used for verification; it does not change the course startup code:

```sh
DOTNET_USE_POLLING_FILE_WATCHER=1 DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false dotnet run --project JustinLee-Lab2/JustinLee-Lab2.csproj
```

## Included

- Overview and analytics with totals calculated from sample records.
- Customer contacts and their one-to-one billing profiles.
- Administrator records inheriting common Person contact fields.
- Subscription plans and configurable software modules (many-to-many).
- Customer subscriptions, API usage, invoices and read-only audit history.
- Create/edit preview forms with custom server-side validation messages.
- Shared responsive layout, navigation, detail pages and accessible form labels.
- [Model and MVC design](docs/DESIGN.md), [screen inventory](docs/SCREENS.md) and [verification](docs/VALIDATION.md).

The source contains 11 domain classes, plus a demo-data container and a dashboard view model. There are 11 concrete controllers and 28 page templates. Shared Razor infrastructure is additional.

## Lab 2 boundary

Forms validate input and display a preview. They intentionally do not save or change the fixed sample dataset. The interface labels this behaviour. Database persistence, real authentication/authorisation, payments, automatic invoice generation and automatic audit capture are future Lab 3 work. The app is a local, unauthenticated coursework prototype.

The proposed ERD describes future persistent relationships. This iteration demonstrates them as linked C# objects; it does not create a database or enforce database constraints. All customer details use fictional names and example.com addresses. Prices are illustrative AUD amounts and exclude tax/proration calculations.

## Verification

With the app running:

```sh
python3 scripts/smoke_test.py
```

The Python script is an optional development check, not an application dependency. It crawls local links, checks missing IDs, exercises valid and invalid forms, confirms previews do not persist, checks HTML encoding and validates displayed totals.

The docs are implementation notes and report material. They are not a claim that the final assessment report has been formatted or submitted.
