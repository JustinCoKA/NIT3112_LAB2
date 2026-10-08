# Verification record

- .NET SDK: 10.0.401.
- Build: `dotnet build JustinLee-Lab2.sln` — 0 warnings, 0 errors.
- HTTP smoke checks: **153 assertions passed**, traversing **45 linked GET routes/assets**.
- Checked all linked pages and records, missing IDs (404), empty forms, valid previews, negative/invalid numeric input, invalid dates, unknown related IDs, invalid statuses, due-date ordering, and invalid module availability.
- Checked that valid previews do not alter sample lists, Razor HTML-encodes submitted text, and dashboard totals match the source records.
- A malformed boolean post initially caused the checkbox helper to throw. The configuration form now uses the lecture-style select control and the malformed input test passes.
- In the restricted macOS environment, file-watcher startup needed per-process polling/reload settings. The application startup remains the Session 3 slide 6 pattern. See README.md for the local run command.

Run `python3 scripts/smoke_test.py` against a running local server to reproduce the checks. Python is optional verification tooling and is not used by the ASP.NET application.

This is not a security audit or a production-readiness claim. No persistent database or operational service integration is implemented for Lab 2.

Browser review confirmed the rendered overview, navigation and customer validation messages. Full-page captures for all 28 page templates are listed in SCREENS.md. A wide-screen layout was not independently verified because the in-app browser retained its pane width.
