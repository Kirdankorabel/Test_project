using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMove_ : MonoBehaviour
{
    private IMover mover;
    public float move = 0.05f;
    public int face = -1;
    public float y;
    public float x;
    int[,] MAP;

    void Awake()
    {
        mover = GetComponent<IMover>();
        MAP = Generator.GetMap();
    }

    void Update()
    {
        if (mover.dirHeld != face)
        {
            Vector3 rPos = mover.pos;
            y = Mathf.Round(rPos.y);
            x = Mathf.Round(rPos.x);

            float delta = 0;

            if (face == 1 || face == 3)
            {
                // Движение по горизонтали, выравнивание по оси y
                delta = y - rPos.y;
            }
            else
            {
                //// Движение по вертикали, выравнивание по оси x
                delta = x - rPos.x;
            }

            if (delta > 0.05 || delta < -0.05)
            {
                float mov = move;
                if (delta < 0) mov = -move;

                if (face == 1 || face == 3)
                {
                    // Движение по горизонтали, выравнивание по оси y
                    rPos.y += mov;
                    delta += mov;
                }
                else if (face == 0 || face == 2)
                {
                    // Движение по вертикали, выравнивание по оси x
                    rPos.x += mov;
                    delta += mov;
                }

                mover.pos = rPos;
            }
            else
            {
                face = mover.dirHeld;
            }
        }
    }
}
