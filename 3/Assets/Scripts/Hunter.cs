using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : TankBot_
{
    float timeInstantiatePoint;
    int x;
    int y;
    float speed_;

    private int[,] Map;

    new void Start()
    {
        base.Start();
        Map = Generator.GetMap();
        speed_ = Speed;
    }

    new void Update()
    {
        if (pX == rX && pY == rY) Speed = 0;
        if (Time.time > timeInstantiatePoint)
        {
            Speed = speed_;
            timeInstantiatePoint = Time.time + 10f;
            InstantiateTargetPoint();
        }

        if (rX == pX && rY == pY)
        {
            Speed = 0;
            GameObject go = TargetsFinder("Player");
            Vector3 pPos = go.transform.position;
            pY = (int)Mathf.Round(pPos.y);
            pX = (int)Mathf.Round(pPos.x);
        }

        PathFinder();
        base.Update();
    }

    void InstantiateTargetPoint()
    {
        int x = Random.Range(1, 15);
        int y = Random.Range(1, 15);
        pX = x;
        pY = y;
        if (Map[x, y] == 1)
        {
            InstantiateTargetPoint();
        }
    }
}
