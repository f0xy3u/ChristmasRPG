using rpgSurvival.invMng;
using rpgSurvival.saveMng;
using rpgSurvival.game;
using rpgSurvival.display;

namespace rpgSurvival
{
    public class player {
        public InventoryManager invMng = new InventoryManager();
        public string name;
        public int health;
        public const int maxHealth = 100;
        public int coins = 30;
        public int[] fightDoneIds = new int[10];
    }
    class Program
    {
        static void Main(string[] args)
        {
            string playerName;
            bool loadGame = false; //used in startUp
            bool gameStarted = false; //used in game loop
            player playerData = new player();
            SaveManager saveMng = new SaveManager();
            BossList bossList = new BossList();
            FightList fightList = new FightList();
            DisplayMenu displayMenu = new DisplayMenu();
            fightSystem fightSystem = new fightSystem();    
            Random random = new Random();
            List<string> weaponInStock = new List<string>();
            List<string> potionInStock = new List<string>();

            void SetUpMenu() {
                //First menu, printed on start and on exit
                displayMenu.showMenu("Hlavní menu", new string[] { "Nová hra", "Načíst hru", "Konec" }, true);
                string choice = displayMenu.selectedIndex.ToString();
                switch(choice){
                    case "0":
                        createNewGame();
                        break;
                    case "1":
                        saveMng.loadSavedGame(ref playerData);
                        loadGame = true;
                        break;
                    case "2":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Neplatná volba.");
                        break;
                }
            }

            void mainMenu() {
                displayMenu.showMenu("Hlavní menu", new string[] { "Boj", "Obchod", "Inventář", "Uložit hru", "Konec" }, false);
                int choice = displayMenu.selectedIndex;
                switch(choice) {
                    case 0:
                        startFight();
                        return;
                    case 1:
                        shop();
                        return;
                    case 2:
                        showInventory();
                        return;
                    case 3:
                        saveMng.saveGame(playerData);
                        return;
                    case 4:
                        Environment.Exit(0);
                        return;
                }
            }

            void shop() {
                displayMenu.showMenu("Obchod", new string[] { "Zbraně", "Lektvary", "Prodej","Zpět" }, false, "Vítej v obchodě, máš " + playerData.coins + " mincí.");
                int choice = displayMenu.selectedIndex;
                bool stocks = false;
                switch(choice) {
                    case 0:
                        stocks = false;
                        if (weaponInStock.Count == 0) {
                            foreach (var weapon in playerData.invMng.weapons) {
                                int chance = random.Next(0, 3);
                                if (chance == 1) {
                                    displayMenu.printText(weapon.Key, $"Cena: {weapon.Value.price}" + " mincí", false, true);
                                    weaponInStock.Add(weapon.Key);
                                    stocks = true;
                                }
                            }
                        } else {
                            foreach (var weapon in weaponInStock) {
                                displayMenu.printText(weapon, $"Cena: {playerData.invMng.weapons[weapon].price}" + " mincí", false, true);
                            }
                            stocks = true;
                        }
                        if (stocks == false) {
                            displayMenu.printText("", "V obchodě není nic k dostání.", false, true);
                            return;
                        }
                        Console.ReadKey();
                        displayMenu.showMenu("Chceš si něco koupit?", new string[] { "Ano", "Ne" }, false);
                        switch (displayMenu.selectedIndex) {
                            case 0:
                                displayMenu.showMenu("Vyber si zbraň", weaponInStock.ToArray());
                                string selectedWeapon = weaponInStock.ToArray()[displayMenu.selectedIndex];
                                if (playerData.coins >= playerData.invMng.weapons[selectedWeapon].price) {
                                    playerData.invMng.weapons[selectedWeapon] = (playerData.invMng.weapons[selectedWeapon].attack, playerData.invMng.weapons[selectedWeapon].amount + 1, playerData.invMng.weapons[selectedWeapon].price);
                                    playerData.coins -= playerData.invMng.weapons[selectedWeapon].price;
                                    displayMenu.printText("", "Zbraň zakoupena.", false, true);
                                } else {
                                    displayMenu.printText("", "Nemáš dostatek mincí.", false, true);
                                }
                                Console.ReadKey();
                                shop();
                                return;
                            case 1:
                                shop();
                                return;
                        }
                        return;
                    case 1:
                        stocks = false;
                        if (potionInStock.Count == 0) {
                            foreach (var potion in playerData.invMng.potions) {
                                int chance = random.Next(0, 3);
                                if (chance == 1) {
                                    displayMenu.printText(potion.Key, $"Cena: {potion.Value.price}" + " mincí", false, true);
                                    potionInStock.Add(potion.Key);
                                    stocks = true;
                                }
                            }
                        } else {
                            foreach (var potion in potionInStock) {
                                displayMenu.printText(potion, $"Cena: {playerData.invMng.weapons[potion].price}" + " mincí", false, true);
                            }
                            stocks = true;
                        }
                        if (stocks == false) {
                            displayMenu.printText("", "V obchodě není nic k dostání.", false, true);
                            return;
                        }
                        Console.ReadKey();
                        displayMenu.showMenu("Chceš si něco koupit?", new string[] { "Ano", "Ne" }, false);
                        switch (displayMenu.selectedIndex) {
                            case 0:
                                displayMenu.showMenu("Vyber si lektvar", potionInStock.ToArray());
                                string selectedPotion = potionInStock.ToArray()[displayMenu.selectedIndex];
                                if (playerData.coins >= playerData.invMng.potions[selectedPotion].price) {
                                    playerData.invMng.potions[selectedPotion] = (playerData.invMng.potions[selectedPotion].health, playerData.invMng.potions[selectedPotion].amount + 1, playerData.invMng.potions[selectedPotion].price);
                                    playerData.coins -= playerData.invMng.potions[selectedPotion].price;
                                    displayMenu.printText("", "Lektvar zakoupen.", false, true);
                                } else { 
                                    displayMenu.printText("", "Nemáš dostatek mincí.", false, true);
                                }
                                shop();
                                return;
                            case 1:
                                shop();
                                return;
                        }
                        return;
                    case 2:
                        List<string> items = new List<string>();
                        foreach (var weapon in playerData.invMng.weapons) {
                            if (weapon.Value.amount > 0) {
                                items.Add(weapon.Key);
                            }
                        }
                        foreach (var potion in playerData.invMng.potions) {
                            if (potion.Value.amount > 0) {
                                items.Add(potion.Key);
                            }
                        }
                        if (items.Count == 0) {
                            displayMenu.printText("", "Nemáš co prodat.", false, true);
                            return;
                        }
                        items.Add("Zpět");
                        displayMenu.showMenu("Vyber si co chceš prodat", items.ToArray());
                        string selectedItem = items.ToArray()[displayMenu.selectedIndex];
                        if (selectedItem == "Zpět") {
                            shop();
                            return;
                        }
                        if (playerData.invMng.weapons.ContainsKey(selectedItem)) {
                            playerData.coins += playerData.invMng.weapons[selectedItem].price / 3 * 2;
                            playerData.invMng.weapons[selectedItem] = (playerData.invMng.weapons[selectedItem].attack, playerData.invMng.weapons[selectedItem].amount - 1, playerData.invMng.weapons[selectedItem].price);
                        } else {
                            playerData.coins += playerData.invMng.potions[selectedItem].price / 3 * 2;
                            playerData.invMng.potions[selectedItem] = (playerData.invMng.potions[selectedItem].health, playerData.invMng.potions[selectedItem].amount - 1, playerData.invMng.potions[selectedItem].price);
                        }
                        displayMenu.printText("", "Předmět prodán.", false, true);
                        Console.ReadKey();
                        mainMenu();
                        return;
                    case 3:
                        mainMenu();
                        return;
                }
            }

            void showInventory() {
                displayMenu.printText("Inventář", "", true, true);
                displayMenu.printText("Tvé statistiky:", "", false, true);
                displayMenu.printText("", $"Jméno:{playerData.name}", false, false);
                displayMenu.printText("", $"Zdraví:{playerData.health}/100", false, false);
                displayMenu.printText("", $"Peníze:{playerData.coins}", false, false);

                displayMenu.printText("Zbraně", "", false, true);
                foreach (var weapon in playerData.invMng.weapons) {
                    if (weapon.Value.amount > 0) {
                        displayMenu.printText($"{weapon.Key}:", $"Počet: {weapon.Value.amount}, Statistiky: Damage: {weapon.Value.attack}", false, false);
                    }
                }
                displayMenu.printText("Lektvary", "", false, true);
                foreach (var potion in playerData.invMng.potions) {
                    if (potion.Value.amount > 0) {
                        displayMenu.printText($"{potion.Key}:", $"Počet: {potion.Value.amount}, Statistiky: Heal: {potion.Value.health}", false, false);
                    }
                }
                Console.ReadKey();
                mainMenu();
            }

            void startFight() {
                bool fightStarted = false;
                bool win = false;
                int i = 0;
                int fightID = 0;
                foreach (var fight in playerData.fightDoneIds) {
                    if(fight == 0 && fightStarted == false) {
                        fightID = i;
                        win = fightSystem.fight(fightList.fights, i, ref playerData, bossList);
                        fightStarted = true;
                    }
                    i++;
                }
                if (win == true) {
                    displayMenu.printText("", "Vyhrál jsi, gratuluji.", false, true);
                    //Pridani penez po vyhre (sum vsech ID*10)
                    Console.WriteLine(fightList.fights[fightID].Sum() * 10);
                    playerData.coins += fightList.fights[fightID].Sum() * 10;
                    Console.ReadKey();
                    playerData.fightDoneIds[fightID] = 1;
                    fightStarted = true;
                    mainMenu();
                    return;
                } else {
                    displayMenu.printText("", "Prohrál jsi, zkus to znovu.", false, true);
                    playerData.health = player.maxHealth/4;
                    Console.ReadKey();
                    mainMenu();
                    return;
                }
            }

            //Game loop
            while (true) {
                //GameLoader
                if(gameStarted == false) {
                    SetUpMenu();
                    startUp();
                    mainMenu();
                    gameStarted = true;
                }
                break;
            }

            //Methods used in game
            void createNewGame() {
                displayMenu.printText("Tvoření postavy", "Jak se chces jmenovat?");
                playerName = Console.ReadLine();
                playerData.name = playerName;
                playerData.health = 100;
            }

            void startUp() {
                if(loadGame == false) {
                    //Load items in inventory
                    playerData.invMng.AddWeapon("Jmelová dýka", 5, 20, 1); 
                    playerData.invMng.AddWeapon("Meč z cukrovinky", 10, 45);
                    playerData.invMng.AddWeapon("Slavnostní luk", 20, 55);
                    playerData.invMng.AddWeapon("Louskáčkový palcát", 25, 70);
                    playerData.invMng.AddWeapon("Sekera mrazivého obra", 35, 90);
                    playerData.invMng.AddWeapon("Hůlka svaté hvězdy", 50, 100);
                    playerData.invMng.AddPotion("Léčivý lektvar", 8, 15, 2);
                    playerData.invMng.AddPotion("Lektvar vánoční pohody", 12, 25);
                    playerData.invMng.AddPotion("Elixír vánočního ducha", 20, 35);
                    playerData.invMng.AddPotion("Vaječný lektvar", 35, 45);
                }
                    
                //Load bosses
                bossList.AddBoss("Vánoční skřítek", 1, 30, 8);
                bossList.AddBoss("Vánoční skřet", 2, 50, 10);
                bossList.AddBoss("Strážce vánočního města", 3, 80, 20);
                bossList.AddBoss("Perníkový golem", 4, 110, 25);
                bossList.AddBoss("Ledová královna", 5, 150, 30);
                bossList.AddBoss("Krampus", 6, 200, 40);
                bossList.AddBoss("Santa", 7, 250, 50);

                if(loadGame == false) {
                    saveMng.saveGame(playerData);
                }

            }
        }
    }
}