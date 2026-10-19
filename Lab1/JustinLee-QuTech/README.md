# Justin Lee QuTech Lab Assessment 1
Student: Justin Lee, S8115784

## Run
Install .NET SDK 10.0 (verified with 10.0.401). Open JustinLee-QuTech.sln in Visual Studio with .NET 10 support, or run from this folder:

    dotnet run --project JustinLee-QuTech/JustinLee-QuTech.csproj

A lists the four plans; B creates a subscription; C lists session subscriptions; D exits.
User count must be 1-100. The code JUST (first four letters of Justin) gives 15% off; No gives no discount. Input is case-insensitive. Monthly total = plan price x users x (discount ? 0.85 : 1).
The student identity is hard-coded as permitted in the brief. There is no authentication or database. Session records are deliberately cleared on exit.
The scenario mentions optional modules but supplies no catalogue, prices or selection task, so modules are outside this implementation.

## Reproduce tests
Python 3 is optional development tooling:

    python3 tests/run_tests.py

12 black-box cases check actual process output and exit status. results.json contains captured output. Evidence images in evidence/ are screenshots of the running .NET application in a live PTY terminal, including actual keyboard inputs. Five entry captures and the five-row summary come from the same process. The summary is followed by a supplementary validation entry and normal exit. Raw terminal output and keyboard events are included.

## Contents
The complete solution, project, C# source, test runner, observed results and evidence are included. Generated bin and obj folders are excluded; dotnet rebuilds them.
