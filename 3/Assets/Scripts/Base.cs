using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    //[Header("Set in Inspector")]
    //public GameObject tankPrefab;
    //public GameObject spawnPrefab;

    //[Header("Set Dynamically")]
    //public int health;

    //public gameObject spawn;
    //public static int numTanks;
    //public static int numTanksOnMap;
    //public static int maxTanks = 30;

    //void Awake()
    //{
    //    spawn = Instantiate<spawn>(spawnPrefab);
    //    spawn.layer = 7;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (numTanksOnMap < 1 && numTanks < maxTanks)
    //    {
    //        GameObject tank = Instantiate<GameObject>(tankPrefab);
    //        if (numTanks % 2 == 0) tank.transform.position = transform.position + Vector3.left;
    //        else tank.transform.position = transform.position - Vector3.left;
    //        tank.transform.SetParent(this.transform);
    //        foreach (Transform transform in spawn)
    //            transform.gameObject.layer = spawn.layer;
    //        numTanks++;
    //        numTanksOnMap++;
    //    }
    //}
}
