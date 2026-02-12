# Flashcard Project
Console application to learn by using flashcards.

Built with **C#**, **SQL Server** + **Dapper**, **Spectre.Console** for clean, colorful menus and **ConsoleTableExt** for tables.

## Requirements

- This is an application where the users will create Stacks of Flashcards.

- You'll need two different tables for stacks and flashcards. The tables should be linked by a foreign key.

- Stacks should have a unique name.

- Every flashcard needs to be part of a stack. If a stack is deleted, the same should happen with the flashcard.

- You should use DTOs to show the flashcards to the user without the Id of the stack it belongs to.

- When showing a stack to the user, the flashcard Ids should always start with 1 without gaps between them. If you have 10 cards and number 5 is deleted, the table should show Ids from 1 to 9.

- After creating the flashcards functionalities, create a "Study Session" area, where the users will study the stacks. Study sessions should be stored, with date and score.

- The study and stack tables should be linked. If a stack is deleted, it's study sessions should be deleted.

- The project should contain a call to the study table so the users can see all their study sessions. This table receives insert calls upon each study session, but there shouldn't be update and delete calls to it.

## Tech Stack

| Component          | Choice                  | Why |
|--------------------|-------------------------|-----|
| Language           | C# / .NET               | Modern, fast, great tooling |
| Database           | SQL Server              | Production-grade, great for learning proper relational DBs |
| ORM                | Dapper                  | Lightweight, fast, no magic |
| UI library         | Spectre.Console         | Beautiful menus and colors |
| Table rendering    | ConsoleTableExt         | Simple & readable static tables |
| Project style      | Clean architecture-ish  | Separation of concerns, DTOs |

## Features

- SQL Server database connection
	- The program uses a SQL Server db connection to store and read information.
    - If no database exists, or the correct table does not exist they will be created on program start.
    - Using Dapper as a ORM to connect and query the database in a cleaner fashion.

- Spectre.Console for the UI, to improve readability.

- Muliple menus for different areas (Stacks, Flashcards, Study Area).

- CRUD DB functions
    - Users can create, read, update or delete Stacks and Flashcards.
    - Users can read and delete Study Sessions.
    - Users can see the exact number of flashcards in each stack.

- Reporting and other data output uses ConsoleTableExt library to output in a more pleasant way.

## Challenges & Lessons Learned

This project pushed me in several directions I hadn't fully explored before:

- **Mastering relational integrity in SQL Server**  
  Properly setting up foreign keys with `ON DELETE CASCADE` so stacks, flashcards, and study sessions stay in sync when deleted.

- **DTOs for clean presentation vs internal models**  
  Using separate DTOs to hide internal IDs (especially StackId) from the user interface. This forced better separation between domain logic and what the user actually sees — no more leaking database concerns into the UI.

- **Switching from SQLite to full SQL Server**  
  Dealing with differences in data types, connection handling, case sensitivity (or lack thereof), and schema creation scripts. Made me appreciate why people say "SQLite for dev, real DB for prod/learning".

- **Building maintainable multi-level menus**  
  Creating intuitive navigation (Main → Stacks → Flashcards → Study Area) without turning the code into spaghetti.

- **Robust input validation & error handling**  
  Making sure the app doesn't crash on empty input, invalid numbers, duplicate stack names, or SQL constraint violations. Added custom validators + friendly error messages.

## Resources

- [Project idea from The C# Sharp Academy](https://www.thecsharpacademy.com/project/14/flashcards)
- Dapper documentation
- SQL Server documentation
