# Verification record

Verified 15 October 2026 using .NET SDK10.0.401, EF Core/Identity10.0.13 and SQLite.

- Build: zero warnings, zero errors.
- Integration: 56/56 tests passed using scripts/lab3_test.py.
- Covered protected routes, login/logout/lockout, CSRF, required/type/boundary validation, six business CRUD workflows, Identity administrator profile/account transaction, billing and configuration, invoice generation/discount/payment/duplicate checks, foreign keys and delete protection, audit entries, and persistence after restart.
- Added boundary checks for December9999 and half-cent rounding (0.05×10%=0.005 rounds to0.01; amount0.04).
- Initial seeded database: five rows per concrete domain class, fifteen plan-module associations, five Identity users and one Admin role.
- SQLite integrity and foreign_key_check passed; initial migration and repeated startup succeeded.
- Captured31 real browser screenshots in a separate evidence database; desktop1440px and mobile390px checked. Mobile page/document width both390px. Tables scroll horizontally in narrow layouts.
- Application excludes tax/proration/external payments as documented. This is local coursework validation, not a production certification.

The final report records test IDs, steps, expected outcomes and actual results. Captures and report are delivered separately from public source.

- Final regression checks passed for stable Identity-ID audit lookup after own-email update, meaningful creation audit targets, and blocking subscription customer reassignment after invoice/usage history.
