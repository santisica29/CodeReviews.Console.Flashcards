using FlashcardsRevisited.Services;
using FlashcardsRevisited.Views;
using System.Configuration;

namespace FlashcardsRevisited;
internal class Program
{
    internal static string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

    static void Main(string[] args)
    {
        DatabaseManager databaseManager = new();
        MainMenu menu = new MainMenu();

        databaseManager.CreateTable(connectionString);
        menu.StartingMenu();
        

    }
}
