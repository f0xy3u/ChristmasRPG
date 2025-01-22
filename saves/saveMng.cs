using System.IO;
using rpgSurvival.display;
using rpgSurvival;
namespace rpgSurvival.saveMng
{
    public class SaveManager
    {
        string path = @"./saves/";
        DisplayMenu displayMenu = new DisplayMenu();

        //Uloží hru do souboru
        //Formát souboru: zdraví \n peníze \n zbraně(název:útok,počet,cena) \n lektvary(název:léčení,počet,cena) \n dokončené souboje
        public void saveGame(player playerData) {
            displayMenu.printText("", "Ukládám hru...", false);
            File.WriteAllText(path + playerData.name + ".rpg", $"{playerData.health}\n{playerData.coins}");
            //Uložení zbraní
            foreach (var item in playerData.invMng.weapons)
            {
                File.AppendAllText(path + playerData.name + ".rpg", $"\n{item.Key}:{item.Value.attack},{item.Value.amount},{item.Value.price}");
            }
            //Uložení lektvarů
            foreach (var item in playerData.invMng.potions)
            {
                File.AppendAllText(path + playerData.name + ".rpg", $"\n{item.Key}:{item.Value.health},{item.Value.amount},{item.Value.price}");
            }
            //Uložení dokončených soubojů
            foreach (var item in playerData.fightDoneIds)
            {
                File.AppendAllText(path + playerData.name + ".rpg", $"\n{item}");
            }
            displayMenu.printText("", $"Hra uložena pro hráče: {playerData.name} s {playerData.health}/100 zdravím.", false);
        }

        //Načte hru ze souboru
        //Formát souboru je stejný jako při ukládání
        public void loadSavedGame(ref player playerData) {
            displayMenu.printText("Načíst save", "");
            
            //Načtení všech uložených her
            string[] saves = Directory.GetFiles(path, "*.rpg");
            displayMenu.showMenu("Vyber save", saves, false);
            string[] playerNameTemp = saves[displayMenu.selectedIndex].Split('.');
            string playerName = playerNameTemp[1];
            playerName = playerName.Remove(0, 7);
            string file = $"{saves[displayMenu.selectedIndex]}";

            if (File.Exists(file)) {
                displayMenu.printText("", "Načítám hru...", false);
                string[] lines = File.ReadAllLines(file);
                //Načtení základních údajů
                playerData.name = playerName;
                playerData.health = Convert.ToInt32(lines[0]);
                playerData.coins = Convert.ToInt32(lines[1]);
                //Načtení zbraní (řádky 2-6)
                for (int i = 2; i <= 6; i++)
                {
                    string[] item = lines[i].Split(':');
                    string[] itemStats = item[1].Split(',');
                    playerData.invMng.AddWeapon(item[0], Convert.ToInt32(itemStats[0]), Convert.ToInt32(itemStats[2]), Convert.ToInt32(itemStats[1]));
                }
                //Načtení lektvarů (řádky 7-11)
                for (int i = 7; i <= 11; i++)
                {
                    string[] item = lines[i].Split(':');
                    string[] itemStats = item[1].Split(',');
                    playerData.invMng.AddPotion(item[0], Convert.ToInt32(itemStats[0]), Convert.ToInt32(itemStats[2]), Convert.ToInt32(itemStats[1]));
                } 
                //Načtení dokončených soubojů (řádky 12-21)
                for (int i = 12; i <= 21; i++)
                {
                    playerData.fightDoneIds[i-12] = Convert.ToInt32(lines[i]);
                }
                displayMenu.printText("", $"Hra načtena pro hráče: {playerData.name} s {playerData.health}/100 zdravím.", false);
            }
        }
    }
}