# Preparation verification

Verified 15 October 2026.

- Baseline commit: e565d86fcf909be253be97059d3bb5784694b416.
- Remote main matched this baseline when preparation started.
- GitHub connector returned pull/push/admin permissions for JustinCoKA on NIT3112_LAB2.
- Fresh clone built using .NET SDK 10.0.401: zero warnings, zero errors.
- scripts/check-baseline.py started its own server on port 5084 and stopped it after checking.
- HTTP smoke suite: 153 assertions passed, 45 linked GET routes/assets.
- Application source has not been changed; no database or Lab 3 features are claimed.
- Report template and student details remain local, outside this public repository.
