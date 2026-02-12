using ConsoleTableExt;
using FlashcardsRevisited.Models;

namespace FlashcardsRevisited.Views;

internal class TableVisualisation
{
    internal static void ShowStacks(List<StackDeck> tableData, string title = "Stacks")
    {
        Console.WriteLine("\n\n");

        var rows = tableData.Select(s => new List<object>
        {
            s.StackName,
            s.Description,
        }).ToList();

        ConsoleTableBuilder
            .From(rows)
            .AddColumn("Name", "Description")
            .WithTitle(title, ConsoleColor.DarkGreen)
            .ExportAndWriteLine();

        Console.WriteLine("\n\n");
    }

    internal static void ShowFlashcards(List<FlashcardDTO> tableData, string title)
    {
        Console.WriteLine("\n\n");
        int id = 1;
        var rows = tableData.Select(f => new List<object>
        {
            id++,
            f.Front,
            f.Back,
        }).ToList();

        ConsoleTableBuilder
            .From(rows)
            .AddColumn("Id", "Front", "Back")
            .WithTitle(title, ConsoleColor.DarkGreen)
            .ExportAndWriteLine();

        Console.WriteLine("\n\n");
    }

    internal static void ShowStudySessions(List<StudySessionDTO> tableData, string title)
    {
        Console.WriteLine("\n\n");

        int id = 1;

        var rows = tableData.Select(ss => new List<object>
        {
            id++,
            ss.StackName,
            ss.DateOfSession.ToString("dd-MM-yyyy"),
            ss.Score,
        }).ToList();

        ConsoleTableBuilder
            .From(rows)
            .AddColumn("Id", "Stack Name", "Date", "Score")
            .WithTitle(title, ConsoleColor.DarkGreen)
            .ExportAndWriteLine();

        Console.WriteLine("\n\n");
    }
}
