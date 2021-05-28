using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stormtrooper_ : TankBot_
{
    new void Update()
    {
        GameObject go = TargetsFinder("Player");
        Vector3 pPos = go.transform.position;
        pY = (int)Mathf.Round(pPos.y);
        pX = (int)Mathf.Round(pPos.x);

        PathFinder();
        base.Update();
    }
}
