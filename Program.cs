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
        public int coins;
        public int[] fightDoneIds = new int[10];
    }
    class Program
    {
        static void Main(string[] args)
        {
            string playerName;
            bool loadGame = false;
            player playerData = new player();
            SaveManager saveMng = new SaveManager();
            BossList bossList = new BossList();
            FightList fightList = new FightList();
            DisplayMenu displayMenu = new DisplayMenu();
            fightSystem fightSystem = new fightSystem();    

            void loadSetUpMenu() {
                //First menu, printed on start and on exit
                displayMenu.showMenu("Hlavní menu", new string[] { "Nová hra", "Načíst hru", "Uložit hru", "Konec" }, true, "cusky brosky");
                string choice = displayMenu.selectedIndex.ToString();
                switch(choice){
                    case "0":
                        createNewGame();
                        break;
                    case "1":
                        saveMng.loadSavedGame(ref playerData.name, ref playerData.health, ref playerData.coins,ref playerData.invMng.weapons,ref playerData.invMng.potions, ref playerData.fightDoneIds);
                        loadGame = true;
                        break;
                    case "2":
                        saveMng.saveGame(playerData.name, playerData.health, playerData.coins, playerData.invMng.weapons, playerData.invMng.potions, playerData.fightDoneIds);
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Neplatná volba.");
                        break;
                }
            }


            //Game loop
            while (true) {
                loadSetUpMenu();
                startUp();
                displayMenu.printText("", "Hra začíná!", false);
                fightSystem.fight(fightList.fights, 0, ref playerData, bossList);
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
                    playerData.invMng.AddWeapon("Jmelová dýka", 5, 1); 
                    playerData.invMng.AddWeapon("Meč z cukrovinky", 10);
                    playerData.invMng.AddWeapon("Slavnostní luk", 20);
                    playerData.invMng.AddWeapon("Louskáčkový palcát", 25);
                    playerData.invMng.AddWeapon("Sekera mrazivého obra", 35);
                    playerData.invMng.AddWeapon("Hůlka svaté hvězdy", 50);
                    playerData.invMng.AddPotion("Léčivý lektvar", 8, 2);
                    playerData.invMng.AddPotion("Lektvar vánoční pohody", 12);
                    playerData.invMng.AddPotion("Elixír vánočního ducha", 20);
                    playerData.invMng.AddPotion("Vaječný lektvar", 35);
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
                    saveMng.saveGame(playerData.name, playerData.health, playerData.coins, playerData.invMng.weapons, playerData.invMng.potions, playerData.fightDoneIds);
                }

            }
        }
    }
}