using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanksSpawn : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject[] tankPrefabs;
    public GameObject spawnPrefab;

    [Header("Set Dynamically")]
    public int health;

    public GameObject spawn;
    public static int numTanks;
    public static int numTanksOnMap;
    public static int maxTanks = 30;

    void Awake()
    {
        spawn = Instantiate<GameObject>(spawnPrefab);
        //spawn.layer = 7; поменять слой
    }

    // Update is called once per frame
    void Update()
    {
        if (numTanksOnMap < 1 && numTanks < maxTanks)
        {
            GameObject tank = Instantiate<GameObject>(tankPrefabs[numTanks % 2]);
            if (numTanks % 2 == 0) tank.transform.position = transform.position + Vector3.left;
            else tank.transform.position = transform.position - Vector3.left;
            tank.transform.SetParent(this.transform);
            if (this.tag == "BlueBase")
            {
                tank.tag = "BlueTank";
            }
            else tank.tag = "RedTank";
            //foreach (Transform transform in this.transform)
            //    transform.gameObject.layer = this.transform.gameObject.layer;
            numTanks++;
            numTanksOnMap++;
        }
    }
}
