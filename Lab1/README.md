# Lab Assessment 1 — QuTech console application

C# console implementation for Justin Lee, S8115784.

- [Source solution and run instructions](JustinLee-QuTech/README.md)
- [Final PDF report](submission/JustinLee_S8115784_Lab1_Report.pdf)
- [Editable Word backup](submission/JustinLee_S8115784_Lab1_Report.docx)
- [Submission source ZIP](submission/JustinLee-Lab1.zip)
- [Observed test results](JustinLee-QuTech/tests/results.json)
- [Execution evidence](JustinLee-QuTech/evidence)

From this directory:

```sh
dotnet run --project JustinLee-QuTech/JustinLee-QuTech/JustinLee-QuTech.csproj
python3 JustinLee-QuTech/tests/run_tests.py
```

Verification: .NET SDK 10.0.401; build with zero warnings and errors; 12/12 black-box cases passed, including a fresh extraction of the submitted ZIP. Five entries exercise all four plans, 1–100 users, the JUST discount and the required summary table. Captures show the running .NET process in an interactive PTY terminal, including actual keyboard inputs, all five entries, the summary, invalid-input recovery and normal exit. Raw terminal output and keyboard events are included.

Lab 2 remains at the repository root. Lab 3 is on the `lab3-preparation` branch.
