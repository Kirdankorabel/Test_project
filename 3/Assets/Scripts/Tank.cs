using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CannonType
{
    none,
    PowerCannons,
    FastCannons
}

[System.Serializable]
public class TankDefinition
{
    public CannonType cannonType = CannonType.none;
    // реализовать тип врага
    public int hullLevel;
    public int towerLevel;
    public int cannonLevel;
}
public class Tank : MonoBehaviour, IMover
{
    [Header("Set in Inspector: Tank")]
    public Loot lootPrefab;
    public Projectile projectilePrefab;
    public GameObject prefabTrack;
    public GameObject[] prefabHulls;
    public GameObject[] prefabTowers;
    public GameObject[] prefabPowerCannons;
    public GameObject[] prefabFastCannons;
    public GameObject visibilityAreaPrefab;
    public int health;
    public int damage;    
    public float rangeAttack;
    //public float Speed;
    public float accuracy;
    public float baseSpeed = 4;
    public float projectileSpeed = 5;
    public float lastShotTime;

    public int facing = 0;
    public Tank tank; //или сделать приватным?

    public Rigidbody rigid;
    public GameObject track;
    public GameObject hull;
    public GameObject tower;
    public GameObject cannon;
    public TankDefinition def;

    private static float showDamageDuration = 0.1f;

    public Vector3[] directions = new Vector3[] {
        Vector3.right,
        Vector3.up,
        Vector3.left,
        Vector3.down };

    public int[] rotations = new int[] {
        0,
        90,
        180,
        270};

    public virtual void Awake()
    {
        facing = 0;
        rigid = GetComponent<Rigidbody>();
    }

    public virtual void Start()
    {
        InstantiateTank();
    }

    public virtual void Update()
    {
        Vector3 vel = Vector3.zero;
        if (dirHeld > -1)
        {
            vel = directions[dirHeld];
            transform.rotation = Quaternion.Euler(0, 0, rotations[dirHeld]);
            facing = dirHeld;
        }
        rigid.velocity = vel * Speed;        
    }

    public virtual void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;
        if (otherGO.tag == "Loot")
        {
            Loot loot = otherGO.GetComponent<Loot>();
            int lootLevel = loot.level;
            string lootType = loot.type;
            Looting(lootLevel, lootType);
            Destroy(otherGO);
        }
    }

    public void TempFire(Vector3 vel)
    {
        Vector3 vec;
        Projectile projGO = Instantiate<Projectile>(projectilePrefab);
        if (Random.value < (1 - accuracy)) vec = new Vector3(0, 0, -1) + transform.position;// промах в зависимости от башни
        else vec = new Vector3(0, 0, 0) + transform.position;
        projGO.transform.position = vec;
        projGO.rangeAttack = rangeAttack;
        projGO.endPosition = vec + rangeAttack * directions[facing];
        projGO.startPosition = vec;
        projGO.damage = damage;
        projGO.transform.rotation = Quaternion.Euler(0, 0, rotations[facing]);
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        projGO.transform.gameObject.layer = this.transform.gameObject.layer;
        rigidB.velocity = vel * projectileSpeed;

        if (this.tag == "BlueTank")
        {
            projGO.tag = "BlueProjectile";
            projGO.GetComponent<Renderer>().material.color = Color.blue;
        }
        else projGO.tag = "RedProjectile";
    }

    public virtual void Looting(int lootLevel, string lootType){ }

    private void InstantiateTank()
    {
        tank = this;
        if (Random.value > 0.5f) def.cannonType = CannonType.FastCannons;
        else def.cannonType = CannonType.PowerCannons;
        Vector3 spawnPosition = transform.position;
        // создание танка
        tank.transform.position = spawnPosition;
        Transform TANK = tank.transform;

        // создание траков
        track = Instantiate<GameObject>(prefabTrack);
        track.transform.SetParent(TANK);
        track.transform.position = TANK.transform.position;

        // создание корпуса
        def.hullLevel = Random.Range(0, prefabHulls.Length);
        hull = Instantiate<GameObject>(prefabHulls[def.hullLevel]);
        hull.transform.SetParent(TANK);
        hull.transform.position = TANK.transform.position;

        // создание башни
        def.towerLevel = Random.Range(0, prefabTowers.Length);
        tower = Instantiate<GameObject>(prefabTowers[def.towerLevel]);
        tower.transform.SetParent(TANK);
        tower.transform.position = TANK.transform.position;

        // создание орудия
        if (def.cannonType == CannonType.FastCannons)
        {
            def.cannonLevel = Random.Range(0, prefabPowerCannons.Length);
            cannon = Instantiate<GameObject>(prefabPowerCannons[def.cannonLevel]);
            cannon.transform.SetParent(TANK);
            cannon.transform.position = TANK.transform.position;
        }
        else
        {
            def.cannonLevel = Random.Range(0, prefabFastCannons.Length);
            cannon = Instantiate<GameObject>(prefabFastCannons[def.cannonLevel]);
            cannon.transform.SetParent(TANK);
            cannon.transform.position = TANK.transform.position;
        }

        charactersDeterminator();
    }

    public void charactersDeterminator()
    {
        // определение характеристик танка
        health = def.hullLevel * 2 + 2;
        Speed = baseSpeed - baseSpeed / 4 * def.hullLevel;
        accuracy = 0.7f + def.towerLevel * 0.1f;
        if (def.cannonType == CannonType.FastCannons )
        {
            rangeAttack = 3 + def.cannonLevel;
            damage = 1 + def.cannonLevel;
        }
        else if (def.cannonType == CannonType.PowerCannons )
        {
            rangeAttack = 4 + 2 * def.cannonLevel;
            damage = 2 + 2 * def.cannonLevel;
        }

        // создание области видимости танка
        GameObject visibilityArea = Instantiate<GameObject>(visibilityAreaPrefab);
        visibilityArea.transform.SetParent(this.transform);
        visibilityArea.transform.position = this.transform.position + new Vector3(rangeAttack / 2 + 0.5f, 0, 0);
        visibilityArea.transform.localScale = new Vector3(rangeAttack, 0, 0);

        foreach (Transform trans in this.transform)
            trans.gameObject.layer = this.transform.gameObject.layer;

        Def = def;
    }

    public TankDefinition Def { get; private set; }//

    // Реализация интерфейса IMover
    public int Facing { get; set; }

    public float Speed { get; set; }

    public Vector3 pos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public int dirHeld { get; set; }
}
