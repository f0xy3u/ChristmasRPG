using System.IO;
using rpgSurvival.display;
using rpgSurvival;
namespace rpgSurvival.saveMng
{
    public class SaveManager
    {
        string path = @"./saves/";
        DisplayMenu displayMenu = new DisplayMenu();
        // Methods
        public void saveGame(player playerData) {
            displayMenu.printText("", "Ukládám hru...", false);
            File.WriteAllText(path + playerData.name + ".rpg", $"{playerData.health}\n{playerData.coins}");
            foreach (var item in playerData.invMng.weapons)
            {
                File.AppendAllText(path + playerData.name + ".rpg", $"\n{item.Key}:{item.Value.attack},{item.Value.amount},{item.Value.price}");
            }
            foreach (var item in playerData.invMng.potions)
            {
                File.AppendAllText(path + playerData.name + ".rpg", $"\n{item.Key}:{item.Value.health},{item.Value.amount},{item.Value.price}");
            }
            foreach (var item in playerData.fightDoneIds)
            {
                File.AppendAllText(path + playerData.name + ".rpg", $"\n{item}");
            }
            displayMenu.printText("", $"Hra uložena pro hráče: {playerData.name} s {playerData.health}/100 zdravím.", false);
        }

        public void loadSavedGame(ref player playerData) {
            displayMenu.printText("Načíst save", "Jak se jmenuje save?");
            string playerName = Console.ReadLine();
            if (File.Exists(path + playerName + ".rpg")) {
                displayMenu.printText("", "Načítám hru...", false);
                string[] lines = File.ReadAllLines(path + playerName + ".rpg");
                playerData.name = playerName;
                playerData.health = Convert.ToInt32(lines[0]);
                playerData.coins = Convert.ToInt32(lines[1]);
                for (int i = 2; i <= 6; i++)
                {
                    string[] item = lines[i].Split(':');
                    string[] itemStats = item[1].Split(',');
                    playerData.invMng.AddWeapon(item[0], Convert.ToInt32(itemStats[0]), Convert.ToInt32(itemStats[2]), Convert.ToInt32(itemStats[1]));
                }
                for (int i = 7; i <= 11; i++)
                {
                    string[] item = lines[i].Split(':');
                    string[] itemStats = item[1].Split(',');
                    playerData.invMng.AddPotion(item[0], Convert.ToInt32(itemStats[0]), Convert.ToInt32(itemStats[2]), Convert.ToInt32(itemStats[1]));
                } 
                for (int i = 12; i <= 21; i++)
                {
                    playerData.fightDoneIds[i-12] = Convert.ToInt32(lines[i]);
                }
                displayMenu.printText("", $"Hra načtena pro hráče: {playerData.name} s {playerData.health}/100 zdravím.", false);
            }
        }
    }
}