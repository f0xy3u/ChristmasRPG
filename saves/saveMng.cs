using System.IO;
using rpgSurvival.display;

namespace rpgSurvival.saveMng
{
    public class SaveManager
    {
        string path = @"./saves/";
        DisplayMenu displayMenu = new DisplayMenu();
        // Methods
        public void saveGame(string name, int health, int coins, Dictionary<string, (int attack, int amount)> weapons, Dictionary<string, (int health, int amount)> potions, int[] fightDoneIds) {
            displayMenu.printText("", "Ukládám hru...", false);
            File.WriteAllText(path + name + ".rpg", $"{health}\n{coins}");
            foreach (var item in weapons)
            {
                File.AppendAllText(path + name + ".rpg", $"\n{item.Key}:{item.Value.attack},{item.Value.amount}");
            }
            foreach (var item in potions)
            {
                File.AppendAllText(path + name + ".rpg", $"\n{item.Key}:{item.Value.health},{item.Value.amount}");
            }
            foreach (var item in fightDoneIds)
            {
                File.AppendAllText(path + name + ".rpg", $"\n{item}");
            }
            displayMenu.printText("", $"Hra uložena pro hráče: {name} s {health}/100 zdravím.", false);
        }

        public void loadSavedGame(ref string name,ref int health,ref int coins , ref Dictionary<string, (int attack, int amount)> weapons, ref Dictionary<string, (int health, int amount)> potions, ref int[] fightDoneIds) {
            displayMenu.printText("Načíst save", "Jak se jmenuje save?");
            string playerName = Console.ReadLine();
            if (File.Exists(path + playerName + ".rpg")) {
                displayMenu.printText("", "Načítám hru...", false);
                string[] lines = File.ReadAllLines(path + playerName + ".rpg");
                name = playerName;
                health = Convert.ToInt32(lines[0]);
                coins = Convert.ToInt32(lines[1]);
                for (int i = 2; i <= 6; i++)
                {
                    string[] item = lines[i].Split(':');
                    string[] itemStats = item[1].Split(',');
                    weapons.Add(item[0], (Convert.ToInt32(itemStats[0]), Convert.ToInt32(itemStats[1])));
                }
                for (int i = 7; i <= 11; i++)
                {
                    string[] item = lines[i].Split(':');
                    string[] itemStats = item[1].Split(',');
                    potions.Add(item[0], (Convert.ToInt32(itemStats[0]), Convert.ToInt32(itemStats[1])));
                }
                for (int i = 12; i <= 21; i++)
                {
                    fightDoneIds[i-12] = Convert.ToInt32(lines[i]);
                }
                displayMenu.printText("", $"Hra načtena pro hráče: {name} s {health}/100 zdravím.", false);
            }
        }
    }
}