# Teaching source map

Application techniques are drawn from the nine user-supplied NIT3112 lecture decks. The actual QuTech Lab3 brief supplies the business requirements. Samples inform report structure only.

| Technique | Course source |
|---|---|
| MVC folders, model/view/controller responsibilities | Session2 slides15–26 |
| C# classes, properties, collections, lambdas | Session1 slides34–43; Session3 slides15,20–22,34–43 |
| Controller actions, routing, View, NotFound | Session4 slides6–13,33–36,44–50 |
| Razor, layouts, loops, ViewBag, partials | Session5 slides4,13,16–21,27–28 |
| Form binding, custom annotations, ModelState | Session6 slides7–18,28–41 |
| EF Core, DbContext, SQLite provider | Session7 slides4–17 and linked EF Core introductory exercises |
| DI database options and connection string | Session7 slides18–20 |
| Model seeding and migration startup | Session7 slides22–29, especially slide26 runtime Migrate |
| Form/input/select/validation tag helpers | Session8 slides20–28 |
| Antiforgery tokens and validation | Session8 slides23–26 |
| IdentityUser, IdentityDbContext, UserManager/SignInManager | Session9 slides4–6,16–21 |
| Authorize/AllowAnonymous and Admin role | Session9 slides5–9,13 |
| Bootstrap responsive styling and login navigation | Session9 slides23–30 |

The slides include older framework listings. The implementation uses the same taught MVC/EF/Identity patterns adapted to the installed .NET10 SDK and version10.0.13 packages. SQLite and all four relationship types are explicitly permitted/required by the actual brief. The local tool manifest supports migration regeneration; normal execution applies the included migration automatically.

No external UI framework, payment SDK, API or database platform is introduced. Python scripts are development verification/report tooling and are not part of the ASP.NET runtime.
