# Teaching-source map

Only the six supplied NIT3112 lecture decks are coding references. The supplied QuTech Lab 2 brief supplies domain requirements. Sample PDFs inform documentation structure, not the business model.

| Implementation | Teaching source |
| --- | --- |
| MVC project and folders | Session 2, slides 15–20, 21–26 |
| Program.cs setup, conventional route | Session 3, slide 6; Session 4, slides 6–13 |
| Classes, properties, methods, constructors | Session 1, slides 34–43; Session 3, slide 15 |
| Object/list initialisers, lambdas | Session 3, slides 20–22, 34–43 |
| Controller action methods, View, NotFound | Session 4, slides 33–36, 44–50 |
| Razor, model values, ViewBag, foreach, conditionals, layout | Session 5, slides 4, 13, 16–21, 27–28 |
| Form binding and HTTP POST | Session 6, slides 7–18 |
| Data annotations with custom ErrorMessage | Session 6, slide 28 |
| Explicit model validation and ModelState | Session 6, slides 29–41 |
| Basic HTML/CSS and Bootstrap presentation | MVC template and the HTML/CSS/Bootstrap examples in Sessions 5–6 |
| One-to-one, one-to-many, many-to-many, inheritance | Explicitly required by the supplied QuTech assessment (prerequisite modelling); represented using the C# classes/properties/collections above |

No EF Core, database migrations, Identity, external UI framework, external API, payment SDK or third-party NuGet package is added. The installed .NET 10 SDK is used, matching the version mentioned in Session 1 slide 4. Session 3 slide 6 supplies the startup style.

The images embedded in the decks remain the authority for the exact examples. Text extraction alone does not capture all of their code. This map identifies the relevant slides, not a claim that every line is copied verbatim.

The course material contains an apparent wording error in Session 6 slide 36: one sentence associates IsValid=true with validation errors. The slide's subsequent explanation and its example intent require valid input to take the success branch. This implementation follows that intent.
