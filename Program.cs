using rpgSurvival.invMng;
using rpgSurvival.saveMng;
using rpgSurvival.game;

namespace rpgSurvival
{
    class player {
        public InventoryManager invMng = new InventoryManager();
        
        public string name;
        public int health;
        public int maxHealth;
        public int coins;
        public int[] fightDoneIds = new int[10];
    }
    class Program
    {
        static void Main(string[] args)
        {
            string playerName;
            player playerData = new player();
            SaveManager saveMng = new SaveManager();
            BossList bossList = new BossList();
            FightList fightList = new FightList();

            //Head name printer
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Vánoční RPG!");
            Console.BackgroundColor = ConsoleColor.Black;

            void loadMenu() {
                //First menu, printed on start and on exit
                Console.WriteLine("1. Nová hra \n2. Načíst hru \n3. Uložit hru \n4. Konec");
                string choice = Console.ReadLine();
                switch(choice){
                    case "1":
                        createNewGame();
                        break;
                    case "2":
                        saveMng.loadSavedGame(ref playerData.name, ref playerData.health, ref playerData.maxHealth, ref playerData.coins,ref playerData.invMng.weapons,ref playerData.invMng.potions, ref playerData.fightDoneIds);
                        break;
                    case "3":
                        saveMng.saveGame(playerData.name, playerData.health, playerData.maxHealth, playerData.coins, playerData.invMng.weapons, playerData.invMng.potions, playerData.fightDoneIds);
                        break;
                    case "4":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Neplatná volba.");
                        break;
                }
            }


            //Game loop
            while (true) {
                loadMenu();
                startUp();
                Console.WriteLine(bossList.Bosses["Vánoční skřítek"]);
                Console.WriteLine("Hra začíná!");
                break;
            }

            //Methods used in game
            void createNewGame() {
                Console.WriteLine("Jak se chces jmenovat?");
                playerName = Console.ReadLine();
                playerData.name = playerName;
                playerData.health = 100;
                playerData.maxHealth = 100;
            }

            void startUp() {
                //Load items in inventory
                playerData.invMng.AddWeapon("Jmelová dýka", 5); 
                playerData.invMng.AddWeapon("Meč z cukrovinky", 10);
                playerData.invMng.AddWeapon("Slavnostní luk", 20);
                playerData.invMng.AddWeapon("Louskáčkový palcát", 25);
                playerData.invMng.AddWeapon("Sekera mrazivého obra", 35);
                playerData.invMng.AddWeapon("Hůlka svaté hvězdy", 50);
                playerData.invMng.AddPotion("Léčivý lektvar", 8);
                playerData.invMng.AddPotion("Lektvar vánoční pohody", 12);
                playerData.invMng.AddPotion("Elixír vánočního ducha", 20);
                playerData.invMng.AddPotion("Vaječný lektvar", 35);
                

                //Load bosses
                bossList.AddBoss("Vánoční skřítek", 1, 30);
                bossList.AddBoss("Vánoční skřet", 2, 70);
                bossList.AddBoss("Strážce vánočního města", 3, 120);
                bossList.AddBoss("Perníkový golem", 4, 175);
                bossList.AddBoss("Ledová královna", 5, 200);
                bossList.AddBoss("Krampus", 6, 250);
                bossList.AddBoss("Santa", 7, 300);

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


                //Save game after startup
                saveMng.saveGame(playerData.name, playerData.health, playerData.coins, playerData.maxHealth, playerData.invMng.weapons, playerData.invMng.potions, playerData.fightDoneIds);
            }
        }
    }
}