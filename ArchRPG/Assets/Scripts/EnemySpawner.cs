using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMap
{
    public EnemyMap()
    {
        ids = new List<int>();
        enemies = new List<unit>();
    }
    List<int> ids;
    List<unit> enemies;
    public void addEnemy(int id, unit bot)
    {
        ids.Add(id);
        enemies.Add(bot);
    }
    public unit getEnemy(int id)
    {
        int index = ids.FindIndex(x => x == id);
        if (index != -1)
        {
            return enemies[index];
        }
        else
        {
            return null;
        }
    }
}

public class EnemySpawner : MonoBehaviour
{
    EnemyMap container;
    int fileNum;


    // Start is called before the first frame update
    void Start()
    {
        
    }
}
