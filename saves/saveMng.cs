using System.IO;

namespace rpgSurvival.saveMng
{
    public class SaveManager
    {
        string path = @"./saves/";
        // Methods
        public void saveGame(string name, int health, int maxHealth, int coins, Dictionary<string, (int attack, int amount)> weapons, Dictionary<string, (int health, int amount)> potions, int[] fightDoneIds) {
            Console.WriteLine("Ukládám hru...");
            File.WriteAllText(path + name + ".rpg", $"{health}\n{maxHealth}\n{coins}");
            foreach (var item in weapons)
            {
                File.AppendAllText(path + name + ".rpg", $"\n{item.Key}:{item.Value}");
            }
            foreach (var item in potions)
            {
                File.AppendAllText(path + name + ".rpg", $"\n{item.Key}:{item.Value}");
            }
            foreach (var item in fightDoneIds)
            {
                File.AppendAllText(path + name + ".rpg", $"\n{item}");
            }
            Console.WriteLine($"Hra uložena pro hráče: {name} s {health}/{maxHealth} zdravím.");
        }

        public void loadSavedGame(ref string name,ref int health,ref int maxHealth,ref int coins , ref Dictionary<string, (int attack, int amount)> weapons, ref Dictionary<string, (int health, int amount)> potions, ref int[] fightDoneIds) {
            Console.WriteLine("Jak se jmenuje save?");
            string playerName = Console.ReadLine();
            if (File.Exists(path + playerName + ".rpg")) {
                Console.WriteLine("Načítám hru...");
                string[] lines = File.ReadAllLines(path + playerName + ".rpg");
                name = playerName;
                health = Convert.ToInt32(lines[0]);
                maxHealth = Convert.ToInt32(lines[1]);
                coins = Convert.ToInt32(lines[2]);
                for (int i = 3; i <= 7; i++)
                {
                    string[] item = lines[i].Split(':');
                    string[] itemStats = item[1].Split(',');
                    weapons.Add(item[0], (Convert.ToInt32(itemStats[0]), Convert.ToInt32(itemStats[1])));
                }
                for (int i = 8; i <= 12; i++)
                {
                    string[] item = lines[i].Split(':');
                    string[] itemStats = item[1].Split(',');
                    potions.Add(item[0], (Convert.ToInt32(itemStats[0]), Convert.ToInt32(itemStats[1])));
                }
                for (int i = 13; i <= 22; i++)
                {
                    fightDoneIds[i-13] = Convert.ToInt32(lines[i]);
                }
                Console.WriteLine($"Hra načtena pro hráče: {name} s {health}/{maxHealth} zdravím.");
            }
        }
    }
}