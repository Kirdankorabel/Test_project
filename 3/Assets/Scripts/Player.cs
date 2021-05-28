using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Tank
{
    private KeyCode[] keys = new KeyCode[] {
        KeyCode.RightArrow,
        KeyCode.UpArrow,
        KeyCode.LeftArrow,
        KeyCode.DownArrow };

    public override void Awake()
    {
        base.Awake();
        rigid = GetComponent<Rigidbody>();
    }

    public override void Start()
    {

        base.Start();
    }

    public override void Update()
    {
        pos = transform.position;
        dirHeld = -1;
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKey(keys[i])) dirHeld = i;
        }

        base.Update();
        Vector3 vel = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TempFire(directions[facing]);
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        //projGO.GetComponent<Renderer>().material.color = Color.blue;
        GameObject otherGO = coll.gameObject;
        if (otherGO.tag == "Loot")
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //добывать название из строки?
            }
        }
        if (otherGO.tag == "BlueProjectile")// банальную булевую проверку?
        {
            Projectile pr = otherGO.GetComponent<Projectile>();
            health -= pr.damage;
            if (health <= 0)
            {
                TanksSpawn.numTanksOnMap--;
                Loot.DropItems(this);
                Loot loot = Instantiate<Loot>(lootPrefab);
                loot.level = Loot.Level;
                loot.type = Loot.Type;
                Destroy(this.gameObject);
            }
            Destroy(otherGO);
        }
        //добавить получение урона, агр союзника
    }
}
