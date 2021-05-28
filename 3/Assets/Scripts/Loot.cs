using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public int level;
    public string type;
    public GameObject loot;
    GameObject item;

    Loot(string s, int i)
    { }

    public static void DropItems(Tank tank)
    {
        int value = Random.Range(0, 3);
        switch (value)
        {
            case 0:
                Type = "hull";
                Level = tank.Def.hullLevel;
                break;
            case 1:
                Type = "tower";
                Level = tank.Def.towerLevel;
                break;
            case 2:
                Type = "cannon";
                Level = tank.Def.cannonLevel;
                break;
        }
    }

    public static int Level { get; set; }
    public static string Type { get; set; }
}
