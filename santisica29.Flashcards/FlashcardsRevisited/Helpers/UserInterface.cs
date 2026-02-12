using Spectre.Console;

namespace FlashcardsRevisited.Helpers;
internal static class UserInterface
{
    internal static void DisplayMessage(string message, string color = "blue")
    {
        AnsiConsole.MarkupLine($"[{color}]{message}[/]");
    }

    internal static string GetStringInput(string message, string color = "blue")
    {
        DisplayMessage(message);
        return Console.ReadLine().Trim().ToLower();
    }

    internal static int GetIntegerInput(string message, string color = "blue")
    {
        string input = GetStringInput(message, color);

        int result;

        while (!int.TryParse(input, out result))
        {
            input = GetStringInput("Invalid input. Enter a number", "red");
        }

        return result;
    }
}
