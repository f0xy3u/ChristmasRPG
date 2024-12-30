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
        public int coins = 50;
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
                displayMenu.showMenu("Hlavní menu", new string[] { "Nová hra", "Načíst hru", "Uložit hru", "Konec" }, true);
                string choice = displayMenu.selectedIndex.ToString();
                switch(choice){
                    case "0":
                        createNewGame();
                        mainMenu();
                        break;
                    case "1":
                        saveMng.loadSavedGame(ref playerData);
                        loadGame = true;
                        mainMenu();
                        break;
                    case "2":
                        saveMng.saveGame(playerData);
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Neplatná volba.");
                        break;
                }
            }

            void mainMenu() {
                displayMenu.showMenu("Hlavní menu", new string[] { "Boj", "Obchod", "Inventář", "Uložit hru", "Konec" }, false, "Vítej v menu");
                int choice = displayMenu.selectedIndex;
                switch(choice) {
                    case 0:
                        //starts fight based on playerData.fightDoneIds
                        return;
                    case 1:
                        shop();
                        return;
                    case 2:
                        //inventory
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
                displayMenu.showMenu("Obchod", new string[] { "Zbraně", "Lektvary", "Zpět" }, false, "Vítej v obchodě, máš " + playerData.coins + " mincí.");
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
                                    displayMenu.printText(potion.Key, $"Cena: {potion.Value.price}" + potion.Value.amount + " mincí", false, true);
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
                                displayMenu.showMenu("Vyber si zbraň", potionInStock.ToArray());
                                string selectedWeapon = potionInStock.ToArray()[displayMenu.selectedIndex];
                                if (playerData.coins >= playerData.invMng.weapons[selectedWeapon].price) {
                                    playerData.invMng.weapons[selectedWeapon] = (playerData.invMng.weapons[selectedWeapon].attack, playerData.invMng.weapons[selectedWeapon].amount + 1, playerData.invMng.weapons[selectedWeapon].price);
                                    playerData.coins -= playerData.invMng.weapons[selectedWeapon].price;
                                    displayMenu.printText("", "Zbraň zakoupena.", false, true);
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
                bossList.AddBoss("Vánoční skřet", 2, 70, 10);
                bossList.AddBoss("Strážce vánočního města", 3, 120, 20);
                bossList.AddBoss("Perníkový golem", 4, 175, 25);
                bossList.AddBoss("Ledová královna", 5, 200, 30);
                bossList.AddBoss("Krampus", 6, 250, 40);
                bossList.AddBoss("Santa", 7, 300, 50);

                //Load fights
                fightList.fights[0] = [1];
                fightList.fights[1] = [2];
                fightList.fights[2] = [1, 2];
                fightList.fights[3] = [3];
                fightList.fights[4] = [1, 2, 3];
                fightList.fights[5] = [1, 1, 2, 3];
                fightList.fights[6] = [4];
                fightList.fights[7] = [2, 2, 5];
                fightList.fights[8] = [6];
                fightList.fights[9] = [7];

                if(loadGame == false) {
                    saveMng.saveGame(playerData);
                }

            }
        }
    }
}