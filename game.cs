using System;
using rpgSurvival.display;
using rpgSurvival;
using rpgSurvival.game;

namespace rpgSurvival.game
{
    public class BossList
    {
        public Dictionary<string, (int id, int health, int dmg)> Bosses = new Dictionary<string, (int id, int health, int dmg)>();

        public void AddBoss(string boss,int id, int health, int dmg)
        {
            Bosses.Add(boss, (id, health, dmg));
        }

        public (string name, int id, int health, int dmg)? getBossByID(int id)
        {
            foreach (var boss in Bosses)
            {
                if (boss.Value.id == id)
                {
                    return (boss.Key, boss.Value.id, boss.Value.health, boss.Value.dmg);
                }
            }
            return null; // Vrátíme null pouze v případě, že žádný boss neodpovídá
        }

    }

    public class FightList 
    {
        public int[][] fights = new int[10][]
        {
        new int[] {1},
        new int[] {2},
        new int[] {1, 2},
        new int[] {3},
        new int[] {1, 2, 3},
        new int[] {1, 1, 2, 3},
        new int[] {4},
        new int[] {2, 2, 5},
        new int[] {6},
        new int[] {7}
        };
    }

    public class fightSystem {
        DisplayMenu displayMenu = new DisplayMenu();
        BossList bossSystem = new BossList();
        Random random = new Random();
        public bool fight(int[][] fights, int fightID, ref player playerData, BossList bossList) {
            int[] bossIDList = fights[fightID];
            Dictionary<string, (int id, int health, int dmg)> Bosses = new Dictionary<string, (int id, int health, int dmg)>();

            foreach (var bossID in bossIDList) {
                //Load bossu
                var boss = bossList.getBossByID(bossID);
                if (boss.HasValue) {
                    Bosses.Add(boss.Value.name, (boss.Value.id, boss.Value.health, boss.Value.dmg));
                } else {
                    Console.WriteLine($"Boss with ID {bossID} not found.");
                }
            }

            //Kontrola zdravi hrace a bosse
            bool skipBoss = false;
            List<string> weaponNames = new List<string>();
            int i = 0; // used only in foreach (72-75)
            foreach (var weapon in playerData.invMng.weapons) {
                if (weapon.Value.amount != 0) {
                    weaponNames.Add(weapon.Key);
                }
                i++;
            }
            displayMenu.showMenu("Vyber si zbraně:", weaponNames.ToArray(), false, "Tuto zbraň budeš používat po celou dobu v boji a nemůžeš ji změnit.");
            var selectedWeapon = playerData.invMng.weapons[weaponNames[displayMenu.selectedIndex]];
              while(playerData.health > 0) {
                skipBoss = false;
                Console.ReadKey();
                foreach (var boss in Bosses) {
                    if (boss.Value.health <= 0) {
                        Bosses.Remove(boss.Key);
                    }
                }
                if (Bosses.Count == 0) {
                    Console.ReadKey();
                    return true;
                }
                //Menu
                displayMenu.showMenu($"Zápas proti bossum", new string[] { "Ukázat stav","Bojovat", "Použít lektvar", "Utéct"}, true);

                //Hráč
                switch (displayMenu.selectedIndex)
                {
                    case 0:
                        // Ukázat stav
                        displayMenu.printText("Tvůj stav:", $"Zdraví: {playerData.health}", true);
                        Console.WriteLine();
                        displayMenu.printText("Stav bosse:", "", false);
                        foreach (var boss in Bosses) {
                            displayMenu.printText(boss.Key, $"Zdraví: {boss.Value.health}", false);
                        }
                        Console.ReadKey();
                        skipBoss = true;
                        break;
                    case 1:
                        // Bojovat
                        int critical = random.Next(0, 5);
                        string[] bossNames = new string[Bosses.Count];
                        int iB = 0; // used only in foreach (110-113)
                        foreach (var boss in Bosses) {
                            bossNames[iB] = boss.Key;
                            iB++;
                        }
                        displayMenu.showMenu("Vyber si protivníka", bossNames);

                        var selectedBoss = Bosses[bossNames[displayMenu.selectedIndex]];
                        if (critical == 1) {
                            displayMenu.printText("Kritický zásah!", $"Zasáhl jsi za {selectedWeapon.attack*2}", true);

                            selectedBoss.health -= selectedWeapon.attack * 2;
                            Bosses[bossNames[displayMenu.selectedIndex]] = selectedBoss;
                        } else {
                            displayMenu.printText("", $"Zasáhl jsi za {selectedWeapon.attack}", true);

                            selectedBoss.health -= selectedWeapon.attack;
                            Bosses[bossNames[displayMenu.selectedIndex]] = selectedBoss;
                        }
                        break;
                    case 2:
                        // Použít lektvar
                        skipBoss = true;
                        List<string> potions = new List<string>();
                        foreach (var potion in playerData.invMng.potions) {
                            if (potion.Value.amount != 0) {
                                potions.Add(potion.Key);
                            }
                        }
                        if (potions.Count == 0) {
                            displayMenu.printText("Nemáš žádné lektvary", "", true);
                            break;
                        }
                        displayMenu.showMenu("Vyber si lektvar", potions.ToArray(), false);
                        var selectedPotion = playerData.invMng.potions[potions[displayMenu.selectedIndex]];
                        displayMenu.printText("Používáš lektvar", $"Použil jsi lektvar a získal jsi {selectedPotion.health} zdraví", true);
                        playerData.health += selectedPotion.health;
                        selectedPotion.amount -= 1;
                        playerData.invMng.potions[potions[displayMenu.selectedIndex]] = selectedPotion;
                        break;
                    case 3:
                        // Utéct
                        displayMenu.showMenu("Opravdu chceš utéct?", new string[] { "Ano", "Ne" }, false);
                        if (displayMenu.selectedIndex == 0) {
                            return false;
                        }
                        break;
                }

                if (skipBoss) {
                    continue;
                }
                //Boss
                foreach(var bossVal in Bosses) {
                    int bossAction = random.Next(0, 2);
                    switch (bossAction)
                    {
                        case 0:
                            // Bojovat
                            int randomVal = random.Next(8, 12);
                            int damage = bossVal.Value.dmg * randomVal / 10;
                            displayMenu.printText($"{bossVal.Key} útočí.", $"{bossVal.Key} ti ubral {damage}", false);
                            playerData.health -= damage;
                            break;
                        case 1:
                            //Miss
                            displayMenu.printText($"{bossVal.Key} se netrefil.", "", false);
                            break;
                    }
                }
                Console.ReadKey();
            }
            if (playerData.health <= 0) {
                return false;
            }
            return true;
        }
    }
}