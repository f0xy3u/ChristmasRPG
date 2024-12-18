using System;
namespace rpgSurvival.display
{
    class DisplayMenu
    {
        public int selectedIndex = 0;
        public int showMenu(string nadpis, string[] options)
        {
            Console.CursorVisible = false;

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkRed;

                string paddedNadpis = $" {nadpis} ";
                int windowWidth = Console.WindowWidth;
                int leftPadding = (windowWidth - paddedNadpis.Length) / 2;

                Console.SetCursorPosition(leftPadding, Console.CursorTop);
                Console.Write(paddedNadpis);

                Console.ResetColor();
                
                Console.WriteLine();
                Console.WriteLine(CenterText("Use arrow keys to navigate and Enter to select:\n"));

                for (int i = 0; i < options.Length; i++)
                {
                    string text = (i == selectedIndex) ? $"> {options[i]}" : $"  {options[i]}";
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    Console.WriteLine(CenterText(text));
                    Console.ResetColor();
                }

                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex == 0) ? options.Length - 1 : selectedIndex - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex == options.Length - 1) ? 0 : selectedIndex + 1;
                        break;
                    case ConsoleKey.Enter:
                        if (options[selectedIndex] == "Exit")
                        {
                            Console.WriteLine(CenterText("Exiting..."));
                            return 0;
                        }
                        return selectedIndex;
                }
            }
        }

        public void printText(string nadpis, string text, bool clear = true)
        {
            if (clear){
                Console.Clear();
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkRed;

            if (nadpis != "") {
                string paddedNadpis = $" {nadpis} ";
                int windowWidth = Console.WindowWidth;
                int leftPadding = (windowWidth - paddedNadpis.Length) / 2;

                Console.SetCursorPosition(leftPadding, Console.CursorTop);
                Console.Write(paddedNadpis);
            }

            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine(CenterText(text));
        }

        private static string CenterText(string text)
        {
            int windowWidth = Console.WindowWidth;
            int textLength = text.Length;
            int leftPadding = (windowWidth - textLength) / 2;
            return new string(' ', Math.Max(leftPadding, 0)) + text;
        }
    }
}