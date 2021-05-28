using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assistant : TankBot_
{
    private int w;
    private int h;
    int x;
    int y;
    private GameObject go;
    private int[,] Map;
    float speed_;
    Vector3 pPos;

    new void Start()
    {
        w = Generator.width;
        h = Generator.height;
        base.Start();
        Map = Generator.GetMap();
        speed_ = Speed;
    }

    new void Update()
    {
        Speed = speed_;
        //go = TargetsFinder("Player");
        //Vector3 pPos = go.transform.position;
        //pY = (int)Mathf.Round(pPos.y);
        //pX = (int)Mathf.Round(pPos.x);
        InstantiateTargetPoint();
        if (rX == pX && rY == pY)
        {
            Speed = 0;
            go = TargetsFinder("BlueTank");
            pPos = go.transform.position;
            pY = (int)Mathf.Round(pPos.y);
            pX = (int)Mathf.Round(pPos.x);
        }
        
        PathFinder();
        base.Update();
    }

    void InstantiateTargetPoint()
    {
        go = TargetsFinder("Player");
        pPos = go.transform.position;
        x = (int)Mathf.Round(pPos.x) - Random.Range(-2, 3);
        y = (int)Mathf.Round(pPos.y) - Random.Range(-2, 3);
        if (x > 0 && x < w && y > 0 && y < h)
        {
            pX = x;
            pY = y;
            if (Map[x, y] == 1)
            {
                InstantiateTargetPoint();
            }
        }
        else InstantiateTargetPoint();
    }
}
