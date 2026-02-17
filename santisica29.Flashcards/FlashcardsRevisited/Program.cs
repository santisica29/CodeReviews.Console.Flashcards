using FlashcardsRevisited.Services;
using FlashcardsRevisited.Views;
using System.Configuration;

namespace FlashcardsRevisited;
internal class Program
{
    

    static void Main(string[] args)
    {
        DatabaseManager databaseManager = new();
        MainMenu menu = new MainMenu();

        databaseManager.Initialize();
        menu.StartingMenu();
        

    }
}
