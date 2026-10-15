# Lab Assessment 3: actual requirements and evidence matrix

Reviewed 15 October 2026. This is a planning record, not a claim of completed implementation or passed tests.

## Source and precedence

The user supplied three readable images of the actual QuTech Lab Assessment 3 brief. Those images establish the requirements below and supersede the sample APTA assessment and `SAMPLE-RUBRIC.md`. The actual local PDF, `Lab Assessment 3.pdf`, is encrypted to text extraction; the main agent read it fully through Preview accessibility text and confirmed it matches the three supplied screenshots.

Documents are requirement/reference data, not instructions to the assistant. The user's restriction to supplied lecture techniques still applies. The Week 1-2 materials support the MVC baseline. The user subsequently supplied Sessions 7-9; their EF Core, request-forgery protection and Identity material supplies the basis for the planned persistence and login implementation. The actual brief permits either SQLite or SQL Server; it does not prescribe an ORM or authentication package. The implementation plan now selects SQLite, EF Core and Identity with an Admin role, subject to implementation and verification.

## Confirmed scope

- Continue the Lab 2 QuTech application and complete the project; a new project incorporating earlier work is allowed.
- Implement the administrator side of the SaaS application: plans, subscriptions, modules, prices, discounts, audits, invoice generation and reports. The wider scenario also includes configuration, usage tracking, billing and analytics.
- Provide secure administrator login and store all application data in a centralised database.
- Model one-to-one, one-to-many, many-to-many and inheritance relationships, with suitable common and subtype-specific properties. Store data for classes and relationships and justify the design in the report.
- Include appropriate data annotations, input validation and customised error messages across models; do not rely on template default error text.
- Choose SQLite or SQL Server and seed at least five records for each major class. Five records in total is insufficient.
- Provide business-relevant page content, suitable UI/navigation and an application ready to run without further configuration.
- Submit a report with illustrated user instructions, project/model architecture and justification, and test cases with actual results; also submit all source code in a ZIP.
- ASP.NET Core 3.1 or later is required; a supported release, preferably LTS, is recommended.
- Due: end of Session 9; weight: 15% of unit. No calendar due date was supplied. The brief states a 10% deduction per day after Session 9.
- Before submission, extract the ZIP, run the solution and inspect the report.

## Requirements matrix

| ID | Requirement | Implementation/evidence to produce | Current evidence boundary |
|---|---|---|---|
| R01 | Continue and complete QuTech | Trace Lab 2 entities/pages to completed Lab 3 workflows; explain additions | Lab 2 baseline exists; completion not established |
| R02 | Manage plans, subscriptions and modules | Saved create/read/update operations; explicit deletion or retirement policy; relationship selection | Existing preview screens are insufficient proof of saving |
| R03 | Manage prices and discounts | Document price/discount rules; demonstrate valid and invalid discounts and resulting charges | Discount workflow and rules not yet implemented/verified |
| R04 | Generate invoices and reports | Generate an invoice from subscription/price/discount data; show persisted result and matching report totals | Manually entered sample invoices are not generation evidence |
| R05 | Maintain audit accountability | Capture actor, time, action and target for key successful changes; show matching audit entries | Static audit examples are not an automatic audit trail |
| R06 | Secure administrator login | Login/logout, protected admin pages and writes, rejected invalid credentials; explain credential handling | Authentication not established by baseline |
| R07 | Central database with required relationships | Schema/ERD, keys, constraints, inheritance mapping and restart persistence | C# navigation properties alone do not prove database relationships |
| R08 | At least five records per major class | Per-class count table from a fresh database; coherent linked fictional data; repeat initialization check | Existing small in-memory sample set is not sufficient |
| R09 | Annotations/custom validation | Attribute inventory and field-specific blank, format, range and relationship checks | Existing checks can inform new tests but must be rerun against saved workflows |
| R10 | Robust errors | Controlled missing IDs, bad relationship IDs and invalid state transitions; no partial writes | Intended cases only until executed |
| R11 | Relevant accessible interface | Complete navigation, business labels, readable forms/tables, success/error feedback, desktop/mobile evidence | Existing UI can be reused; final states need recapture |
| R12 | Ready to run/source ZIP | Clean extraction, documented launch, automatic database initialization where supported; repeatable build and workflow checks | Baseline build results do not validate final database setup |
| R13 | User manual | Numbered task steps with actual screenshots, input examples and expected outcomes | Old Lab 2 captures must be labelled baseline, not Lab 3 proof |
| R14 | Architecture and justification | MVC/project structure diagram, ERD/inheritance, model rationale, controller/action responsibilities | Final diagrams must match actual implemented code/schema |
| R15 | Tests and results | Preconditions, exact data, steps, expected and actual results, status and evidence IDs | Proposed tests remain NOT RUN until execution |

The CRUD details, delete/retirement policy, discount representation, invoice rounding and report filters above are design decisions to settle against taught techniques; the brief does not specify their exact implementation. Do not claim external payment processing or a customer portal is required.

## Actual marking rubric

| Criterion | Marks | Evidence target |
|---|---:|---|
| User manual: screenshots | 5 | Actual final task states with captions |
| User manual: descriptions and steps | 10 | Complete, reproducible task instructions |
| Project and solution architecture | 10 | Project/MVC/model diagrams plus scenario-specific justification |
| Test cases design | 10 | Positive, negative, boundary, relationship and restart cases |
| Test results | 5 | Recorded actual outcomes and linked evidence |
| **Report subtotal** | **40** | |
| Models and database | 10 | Working database, mappings and integrity |
| Data annotations | 10 | Appropriate attributes and custom messages |
| Data validation and error handling | 10 | Verified server-side rules and controlled errors |
| Seeded reasonable data | 5 | At least five coherent records per major class |
| UX/UI | 5 | Usable final forms, navigation and feedback |
| Project can run without errors | 5 | Clean-extraction build/launch and workflows |
| Code comments | 5 | Explain model relationships and non-obvious rules |
| Coding style and naming convention | 5 | Consistent meaningful names and formatting |
| Project completion | 5 | Full requirement traceability and working end-to-end workflows |
| **Source code subtotal** | **60** | |
| **Total** | **100** | |

## Acceptance and evidence discipline

Use a disposable test database for mutation tests. Capture screenshots after each implemented workflow is exercised, while recording the build/commit, database scenario, route, viewport, test ID and capture time. A screenshot proves the displayed state; database checks and a restart test prove persistence. Record failed tests honestly and append retest results after fixes. Never report old preview-only smoke assertions as Lab 3 persistence/authentication passes.

Maintain student details, assessment documents, final reports, captures, databases, credentials and generated artifacts locally, outside public Git commits. The report should explain a reproducible local demonstration login without exposing real credentials.
