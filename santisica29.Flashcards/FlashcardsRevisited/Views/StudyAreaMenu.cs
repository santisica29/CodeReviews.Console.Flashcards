using FlashcardsRevisited.Controllers;
using FlashcardsRevisited.Models;
using Spectre.Console;
using static FlashcardsRevisited.Helpers.UserInterface;

namespace FlashcardsRevisited.Views;
internal class StudyAreaMenu
{
    private readonly StudySessionController _studySessionController;
    private readonly StackController _stackController;
    private readonly FlashcardController _flashcardController;
    private StackDeck? _currentStack;
    public StudyAreaMenu(StackController stackController, FlashcardController flashcardController)
    {
        _stackController = stackController;
        _flashcardController = flashcardController;
        _studySessionController = new();
    }
    internal void MainMenu()
    {
        bool closeApp = false;

        while (!closeApp)
        {
            Console.Clear();

            Console.WriteLine("Study Area");
            Console.WriteLine("0 - Go back");
            Console.WriteLine("1 - Start study session");
            Console.WriteLine("2 - View study sessions in a stack");
            Console.WriteLine("3 - View all study sessions");
            Console.WriteLine("4 - Change current stack");

            string userChoice = Console.ReadLine().Trim().ToLower();

            switch (userChoice)
            {
                case "0":
                    closeApp = true;
                    break;
                case "1":
                    _currentStack ??= ChooseCurrentStack();
                    Console.WriteLine($"Current stack: {_currentStack.StackName}\n");
                    ProcessStartStudySession();
                    break;
                case "2":
                    //if (left) is null, assign (right)
                    _currentStack ??= ChooseCurrentStack();
                    Console.WriteLine($"Current stack: {_currentStack.StackName}\n");
                    ProcessViewStudySessionsInStack();
                    break;
                case "3":
                    ProcessViewAllStudySessions();
                    break;
                case "4":
                    Console.WriteLine($"Current stack: {_currentStack.StackName}\n");
                    _currentStack = ChooseCurrentStack();
                    break;
            }

            Console.ReadKey();
        }
    }

    private void ProcessViewAllStudySessions()
    {
        List<StudySessionDTO> list = _studySessionController.GetAll();

        if (list == null)
            DisplayMessage("No study sessions.", "red");
        else
            TableVisualisation.ShowStudySessions(list, "Study Sessions");
    }

    internal void ProcessStartStudySession()
    {
        var flashcardList = _flashcardController.GetFlashcardsFromStack(_currentStack.StackId);

        if (flashcardList.Count == 0)
        {
            DisplayMessage("No flashcards found in this stack", "red");
            return;
        }

        int score = 0;

        foreach (var flashcard in flashcardList)
        {
            string answer = GetStringInput($"{flashcard.Front}:");

            if (answer == flashcard.Back)
            {
                score++;
                DisplayMessage("Correct answer", "green");
            }
            else
            {
                DisplayMessage($"Incorrect answer. The correct answer was: {flashcard.Back}", "red");

            }

            Console.ReadKey();
        }

        DisplayMessage($"Final score {score}/{flashcardList.Count} pts");
        Console.ReadKey();

        var saveSession = AnsiConsole.Confirm("Do you want to save this session?");

        if (!saveSession)
        {
            DisplayMessage("The session won't be saved. Press any key.", "red");
            Console.ReadKey();
            return;
        }

        StudySession newSession = new()
        {
            Score = score,
            DateOfSession = DateTime.Now,
            Stack = _currentStack,
        };

        var affectedRows = _studySessionController.Add(newSession);

        if (affectedRows > 0)
            DisplayMessage("Study session added correctly!", "green");
        else
            DisplayMessage("Couldn't saved study session", "red");
    }

    internal void ProcessViewStudySessionsInStack()
    {
        var sessions = _studySessionController.GetSessionsFromStack(_currentStack.StackId);

        if (sessions.Count == 0)
            DisplayMessage("No sessions found in this stack", "red");
        else
            TableVisualisation.ShowStudySessions(sessions, $"{_currentStack.StackName} sessions");
    }

    internal StackDeck? ChooseCurrentStack()
    {
        var listOfStacks = _stackController.GetAll();

        if (listOfStacks.Count == 0)
        {
            DisplayMessage("No stacks found.", "red");
            return null;
        }

        TableVisualisation.ShowStacks(listOfStacks);

        StackDeck? currStack = null;

        while (currStack == null)
        {
            string stackName = GetStringInput("Type the name of the Stack you want:", "yellow");

            currStack = _stackController.GetByName(stackName);

            if (currStack == null)
                DisplayMessage("Stack not found. Try again.", "red");
        }

        return currStack;
    }
}
