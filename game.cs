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
        new int[4],
        new int[4],
        new int[4],
        new int[4],
        new int[4],
        new int[4],
        new int[4],
        new int[4],
        new int[4],
        new int[4]
        };
    }

    public class fightSystem {
        DisplayMenu displayMenu = new DisplayMenu();
        BossList bossSystem = new BossList();
        Random random = new Random();
        public void fight(int[][] fights, int fightID, ref player playerData, BossList bossList) {
            int[] bossIDList = fights[fightID];
            Dictionary<string, (int id, int health, int dmg)> Bosses = new Dictionary<string, (int id, int health, int dmg)>();

            foreach (var bossID in bossIDList) {
                //Load bossů
                if (bossID == 0) continue;

                var boss = bossList.getBossByID(bossID);
                if (boss.HasValue) {
                    Bosses.Add(boss.Value.name, (boss.Value.id, boss.Value.health, boss.Value.dmg));
                } else {
                    Console.WriteLine($"Boss with ID {bossID} not found.");
                }
            }

            //Kontrola zdravi hrace a bosse
            bool skipBoss = false;
            while(playerData.health > 0) {
                skipBoss = false;
                foreach (var boss in Bosses) {
                    if (boss.Value.health <= 0) {
                        Bosses.Remove(boss.Key);
                    }
                }
                if (Bosses.Count == 0) {
                    break;
                }
                //Menu
                displayMenu.showMenu($"Zápas proti {"idk"}", new string[] { "Ukázat stav","Bojovat", "Použít lektvar", "Utéct"}, true);

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
                        int i = 0;
                        foreach (var boss in Bosses) {
                            bossNames[i] = boss.Key;
                            i++;
                        }
                        displayMenu.showMenu("Vyber si protivníka", bossNames);

                        var selectedBoss = Bosses[bossNames[displayMenu.selectedIndex]];
                        if (critical == 1) {
                            displayMenu.printText("Kritický zásah!", "Zasáhl jsi za 10", true);

                            selectedBoss.health -= 10;
                            Bosses[bossNames[displayMenu.selectedIndex]] = selectedBoss;
                        } else {
                            displayMenu.printText("", "Zasáhl jsi za 5", true);

                            selectedBoss.health -= 5;
                            Bosses[bossNames[displayMenu.selectedIndex]] = selectedBoss;
                        }
                        break;
                    case 2:
                        // Použít lektvar
                        Console.WriteLine("Používám lektvar");
                        break;
                    case 3:
                        // Utéct
                        Console.WriteLine("Utíkám");
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
                            Console.WriteLine(randomVal);
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
        }
    }
}