# Lab Assessment 1 — QuTech console application

C# console implementation for Justin Lee, S8115784.

- [Source solution and run instructions](JustinLee-QuTech/README.md)
- [Word report](submission/JustinLee_S8115784_Lab1_Report.docx)
- [Submission source ZIP](submission/JustinLee-Lab1.zip)
- [Observed test results](JustinLee-QuTech/tests/results.json)
- [Execution evidence](JustinLee-QuTech/evidence)

From this directory:

```sh
dotnet run --project JustinLee-QuTech/JustinLee-QuTech/JustinLee-QuTech.csproj
python3 JustinLee-QuTech/tests/run_tests.py
```

Verification: .NET SDK 10.0.401; build with zero warnings and errors; 12/12 black-box cases passed, including a fresh extraction of the submitted ZIP. Five entries exercise all four plans, 1–100 users, the JUST discount and the required summary table. Captures show unchanged console output in an execution log viewer; they are not native Terminal screenshots.

Lab 2 remains at the repository root. Lab 3 is on the `lab3-preparation` branch.
