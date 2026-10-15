# Lab 3 preparation

Prepared 15 October 2026. This is not a completed Lab 3 submission.

## Current starting point

Repository: JustinCoKA/NIT3112_LAB2; branch: lab3-preparation.
Application: JustinLee-Lab2.sln / JustinLee-Lab2/JustinLee-Lab2.csproj.
Target framework: net10.0; SDK: 10.0.401 (global.json permits later 10.0.4xx patches).
The separate local checkout is named NIT3112_LAB3. Project renaming awaits the actual brief.
The app retains QuTech models, 11 controllers, 28 page templates and preview-only sample data.
No database provider, EF package or authentication library has been selected or installed.

## Start immediately

Run from the checkout root:

```sh
bash scripts/run-baseline.sh
```

Open http://127.0.0.1:5083. Stop the server with Ctrl+C.
To rebuild and test an independently started temporary server:

```sh
python3 scripts/check-baseline.py
```

The check script builds, starts the baseline on port 5084, runs the existing HTTP smoke checks, and stops only its own server. It refuses to use an occupied port. Python is verification tooling, not application code.

## Decisions required from the actual assessment

1. Confirm scenario, rubric, deadline, project name and required submission formats.
2. Confirm permitted lecture techniques and additional lecture materials.
3. Confirm database engine, ORM/mapping approach and installation requirements.
4. Confirm required persistence, CRUD, relationships, authentication and audit behaviour.
5. Replace the provisional sample rubric mapping and test expectations with actual criteria.

The supplied sample is internally inconsistent: its body references Assessment 2 and incomplete UI work, while its rubric awards marks for models/database, seeding, test design/results and a user manual. It also uses an APTA scenario and a report with unrelated club screenshots. These examples do not establish the actual Lab 3 domain or feature scope.

## Implementation order once confirmed

1. Map actual requirements to the existing entities and UI; identify changes.
2. Select taught persistence techniques and document PK/FK/inheritance choices.
3. Implement schema and reproducible fictional seed data.
4. Replace preview actions with required persistent operations; add error handling.
5. Run positive, negative, boundary, relationship and persistence checks.
6. Capture actual final UI states and fill the user manual and results.
7. Extract a clean submission ZIP and verify build, database setup and execution.

## Evidence workflow

TEST-PLAN.csv contains proposed cases, all marked NOT RUN. Add steps/data and actual requirement IDs before execution. EVIDENCE.csv identifies planned screenshots, all NOT CAPTURED.
The existing Lab 2 suite expects no persistence. Retain it as historical baseline verification; do not count it as proof of Lab 3 persistence.
The report template is local in outputs/NIT3112_LAB3_Preparation. It includes student details and is not published to this public repository.
