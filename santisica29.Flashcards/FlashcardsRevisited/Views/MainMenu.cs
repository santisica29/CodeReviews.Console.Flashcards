using FlashcardsRevisited.Controllers;

namespace FlashcardsRevisited.Views
{
    internal class MainMenu
    {
        private readonly StackController _stackController;
        private readonly FlashcardController _flashcardController;

        private readonly StacksMenu _stacksMenu;
        private readonly FlashcardsMenu _flashcardsMenu;
        private readonly StudyAreaMenu _studyAreaMenu;

        public MainMenu()
        {
            _flashcardController = new FlashcardController();
            _stackController = new StackController();
            _stacksMenu = new StacksMenu(_stackController);
            _flashcardsMenu = new FlashcardsMenu(_stackController);
            _studyAreaMenu = new StudyAreaMenu(_stackController, _flashcardController);
        }

        internal void StartingMenu()
        {
            bool closeApp = false;

            while (!closeApp)
            {
                Console.Clear();

                Console.WriteLine("Main Menu.");
                Console.WriteLine("-----------");
                Console.WriteLine("Manage Stacks (press s)");
                Console.WriteLine("Manage Flashcards (press f)");
                Console.WriteLine("Study Area (press x)");
                Console.WriteLine("Exit (press 0)");
                Console.WriteLine("-----------");
                Console.WriteLine("Choose one.");

                var commandInput = Console.ReadLine().Trim().ToLower();

                switch (commandInput)
                {
                    case "s":
                        _stacksMenu.MainMenu();
                        break;
                    case "f":
                        _flashcardsMenu.MainMenu();
                        break;
                    case "x":
                        _studyAreaMenu.MainMenu();
                        break;
                    case "0":
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid input.");
                        break;
                }
            }
        }
    }
}
