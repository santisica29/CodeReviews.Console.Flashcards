using FlashcardsRevisited.Controllers;
using FlashcardsRevisited.Models;
using Microsoft.IdentityModel.Tokens;
using Spectre.Console;
using System.Linq;
using static FlashcardsRevisited.Helpers.UserInterface;

namespace FlashcardsRevisited.Views;

internal class StacksMenu
{
    private StackController _stackController;

    public StacksMenu(StackController stackController)
    {
        _stackController = stackController;
    }

    internal void MainMenu()
    {
        bool closeStackMenu = false;

        while (!closeStackMenu)
        {
            Console.Clear();

            Console.WriteLine("Stacks Menu.");
            Console.WriteLine("1 - View All");
            Console.WriteLine("2 - Create");
            Console.WriteLine("3 - Update");
            Console.WriteLine("4 - Delete");
            Console.WriteLine("0 - Go Back");

            string userInput = GetStringInput("Select your option:");

            switch (userInput)
            {
                case "1":
                    ProcessViewStacks();
                    break;
                case "2":
                    ProcessCreateStack();
                    break;
                case "3":
                    ProcessUpdateStack();
                    break;
                case "4":
                    ProcessDeleteStack();
                    break;
                case "0":
                    closeStackMenu = true;
                    break;
                default:
                    DisplayMessage("Invalid input", "red");
                    break;
            }

            Console.ReadKey();
        }
    }

    internal void ProcessDeleteStack()
    {
        ProcessViewStacks();

        string nameSelected = GetStringInput("Type the name of the Stack you want to delete");

        StackDeck? stackToDelete = _stackController.GetByName(nameSelected);

        if (stackToDelete == null)
        {
            DisplayMessage($"No Stack found called {nameSelected}.", "red");
            Console.ReadKey();
            return;
        }

        bool confirmationInput = AnsiConsole.Confirm($"Are you sure you want to delete {stackToDelete.StackName}?");

        if (!confirmationInput)
        {
            DisplayMessage("Deletion canceled.");
            Console.ReadKey();
            return;
        }

        int rowsAffected = _stackController.Delete(stackToDelete.StackId);

        if (rowsAffected > 0)
            DisplayMessage("Stack deleted", "green");
        else
            DisplayMessage("Couldn't delete the Stack", "red");
    }

    private void ProcessUpdateStack()
    {
        ProcessViewStacks();

        string nameSelected = GetStringInput("Type the name of the Stack you want to update");

        StackDeck? stackToUpdate = _stackController.GetByName(nameSelected);

        if (stackToUpdate == null)
        {
            DisplayMessage("No Stack found with that name. Try Again", "red");
            Console.ReadKey();

            Console.Clear();
            ProcessUpdateStack();
            return;
        }

        bool updating = true;

        while (updating)
        {
            Console.Clear();

            Console.WriteLine("Select which part you want to update.");
            Console.WriteLine("Press 1 to update the name");
            Console.WriteLine("Press 2 to update the description");
            Console.WriteLine("Press s to stop updating");
            Console.WriteLine("Press 0 to go back");

            var cmd = Console.ReadLine().Trim().ToLower();

            switch (cmd)
            {
                case "1":
                    string newName = GetStringInput("Enter a new name");
                    stackToUpdate.StackName = newName;
                    break;
                case "2":
                    string newDescription = GetStringInput("Enter a new description");
                    stackToUpdate.Description = newDescription;
                    break;
                case "s":
                    updating = false;
                    break;
                case "0":
                    MainMenu();
                    return;    
            }

            Console.ReadKey();
        }

        int rowsAffected = _stackController.Update(stackToUpdate);

        if (rowsAffected > 0)
            DisplayMessage($"Stack with the name of {stackToUpdate.StackName} was updated.", "green");
        else
            DisplayMessage("Couldn't update the stack.", "red");
    }

    private void ProcessCreateStack()
    {
        var listOfStacks = _stackController.GetAll();

        DisplayMessage("Create a new stack.");
        DisplayMessage("------------------");

        string nameInput = GetStringInput("Name (mandatory):");

        while (nameInput.IsNullOrEmpty() || listOfStacks.Any(s => s.StackName.Equals(nameInput)))
        {
            DisplayMessage("Invalid input, the name can't be empty or another stack with the same name was found", "red");
            nameInput = GetStringInput("Name (mandatory):");
        }

        string descriptionInput = GetStringInput("Description (optional):");

        StackDeck newStack = new()
        {
            StackName = nameInput,
            Description = descriptionInput,
            CreatedDate = DateTime.Now,
        };

        int rowsAffected = _stackController.Add(newStack);

        if (rowsAffected > 0)
            DisplayMessage("New Stack created!", "green");
        else
            DisplayMessage("Couldn't create the stack", "red");
    }

    private void ProcessViewStacks()
    {
        var listOfStacks = _stackController.GetAll();

        if (listOfStacks.Count == 0)
            DisplayMessage("No stacks found.", "red");
        else
            TableVisualisation.ShowStacks(listOfStacks, "Stacks");

        DisplayMessage("Press any key");
    }
}
