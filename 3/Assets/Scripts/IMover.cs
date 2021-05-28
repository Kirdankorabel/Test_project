using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMover
{
    int dirHeld { get; set; }
    int Facing { get; set; }
    float Speed { get; set; }
    Vector3 pos { get; set; }
}
