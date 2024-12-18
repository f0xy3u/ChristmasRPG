using System;

namespace rpgSurvival.game
{
    public class BossList
    {
        public Dictionary<string, (int id, int health)> Bosses = new Dictionary<string, (int id, int health)>();

        public void AddBoss(string boss,int id, int health)
        {
            Bosses.Add(boss, (id, health));
        }

        public (string name, int health)? getBossByID(int id)
        {
            foreach (var boss in Bosses)
            {
                if (boss.Value.id == id)
                {
                    return (boss.Key, boss.Value.health);
                } else {
                    return null;
                }
            }
            return null;
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
}