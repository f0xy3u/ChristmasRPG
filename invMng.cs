namespace rpgSurvival.invMng
{
    public class InventoryManager
    {
        public Dictionary<string, (int attack, int amount)> weapons = new Dictionary<string, (int attack, int amount)>();
        public Dictionary<string, (int health, int amount)> potions = new Dictionary<string, (int health, int amount)>();

        // Methods
        public void AddWeapon(string item, int attack, int amount = 0)
        {
            weapons.Add(item, (attack, amount));
        }

        public void AddPotion(string item, int health, int amount = 0)
        {
            potions.Add(item, (health, amount));
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