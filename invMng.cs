namespace rpgSurvival.invMng
{
    public class InventoryManager
    {
        public Dictionary<string, (int attack, int amount, int price)> weapons = new Dictionary<string, (int attack, int amount, int price)>();
        public Dictionary<string, (int health, int amount, int price)> potions = new Dictionary<string, (int health, int amount, int price)>();

        // Methods
        public void AddWeapon(string item, int attack, int price, int amount = 0)
        {
            weapons.Add(item, (attack, amount, price));
        }

        public void AddPotion(string item, int health, int price, int amount = 0)
        {
            potions.Add(item, (health, amount, price));
        }

        public int HasWeapon(string item)
        {
            return weapons[item].amount;
        }

        public int HasPotion(string item)
        {
            return potions[item].amount;
        }
    }
}