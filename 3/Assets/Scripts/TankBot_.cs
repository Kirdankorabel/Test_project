using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBot_ : Tank
{
    private GameObject generator;
    private string[] RedTeamTags = { "RedTank", "Player", "RedBase" };
    private string[] BlueTeamTags = { "BlueTank", "BlueBase", "Attacker" };

    private int width;
    private int height;
    public bool attakPlayer;

    private int[,] cMap;
    private int[,] Map;

    public override void Awake()
    {
        width = Generator.width;
        height = Generator.height;
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        Map = Generator.GetMap();

        cMap = Map;
    }

    public new void Update()//override
    {
        Move();
        base.Update();
    }

    public void OnTriggerStay(Collider coll)
    {
        GameObject otherGO = coll.gameObject;
        if (otherGO.tag == "RedTank" && Time.time > lastShotTime)
        {
            lastShotTime = Time.time + 0.5f;// сделать задержку настраиваемой. или не сделать
            TempFire(directions[facing]);
        }
    }

    public override void OnCollisionEnter(Collision coll)//
    {
        base.OnCollisionEnter(coll);
        GameObject otherGO = coll.gameObject;
        if (otherGO.tag == "RedProjectile")// банальную булевую проверку?
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
                ItemDroper(loot);
                Destroy(this.gameObject);
            }
            Destroy(otherGO);
        }
    }

    public void PathFinder()
    {
        // позиция танка
        pos = transform.position;
        rY = (int)Mathf.Round(pos.y);
        rX = (int)Mathf.Round(pos.x);

        while (cMap[rX, rY] > -1)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == pX && y == pY && cMap[x, y] > -1) cMap[x, y] = -1;

                    if (cMap[x, y] < -1)
                    {
                        cMap[x, y] = cMap[x, y] - 1;
                    }

                    if (cMap[x, y] == -1)
                    {
                        cMap[x, y]--;
                        if (cMap[x - 1, y] == 0) cMap[x - 1, y] = 2;
                        if (cMap[x + 1, y] == 0) cMap[x + 1, y] = 2;
                        if (cMap[x, y + 1] == 0) cMap[x, y + 1] = 2;
                        if (cMap[x, y - 1] == 0) cMap[x, y - 1] = 2;
                    }
                }
            }
            for (int x = width - 1; x > 0; x--)
            {
                for (int y = height - 1; y > 0; y--)
                {
                    if (cMap[x, y] == 2)
                    {
                        cMap[x, y] = -1;
                    }
                }
            }
            if (cMap[rX, rY] < 0) return;
        }
    }
        

    void Move()
    {
        if (cMap[rX, rY] == -1)
        {
            for (int x = 0; x < width - 1; x++)
            {
                for (int y = 0; y < height - 1; y++)
                {
                    if (cMap[x, y] < 0) cMap[x, y]++;
                }
            }
            if (cMap[rX + 1, rY] == -1) dirHeld = 0;
            if (cMap[rX, rY + 1] == -1) dirHeld = 1;
            if (cMap[rX - 1, rY] == -1) dirHeld = 2;
            if (cMap[rX, rY - 1] == -1) dirHeld = 3;
        }
        //if (rX == pX && rY == pY)
        //{
        //    dirHeld = (dirHeld + 2) % 4;
        //}
    }

    public override void Looting(int lootLevel, string lootType)
    {
        switch (lootType)
        {
            case "hull":
                if (lootLevel > this.def.hullLevel)
                {
                    Transform TANK = tank.transform;
                    Destroy(hull);
                    this.def.hullLevel = lootLevel;
                    hull = Instantiate<GameObject>(prefabHulls[lootLevel]);
                    hull.transform.SetParent(TANK);
                    hull.transform.position = TANK.transform.position;
                    hull.transform.rotation = Quaternion.Euler(0, rotations[facing], 0);
                }
                break;
            case "tower":
                if (lootLevel > this.def.towerLevel)
                {
                    Transform TANK = tank.transform;
                    Destroy(tower);
                    this.def.towerLevel = lootLevel;
                    tower = Instantiate<GameObject>(prefabTowers[lootLevel]);
                    tower.transform.SetParent(TANK);
                    tower.transform.position = TANK.transform.position + new Vector3(0, 0.2f, 0);
                    tower.transform.rotation = Quaternion.Euler(0, rotations[facing], 0);
                }
                break;
            case "cannon":
                if (lootLevel > this.def.cannonLevel)
                {
                    Transform TANK = tank.transform;
                    Destroy(cannon);
                    this.def.cannonLevel = lootLevel;
                    if (def.cannonType == CannonType.FastCannons)
                    {
                        cannon = Instantiate<GameObject>(prefabPowerCannons[def.cannonLevel]);
                        cannon.transform.SetParent(TANK);
                        cannon.transform.position = TANK.transform.position;
                    }
                    else
                    {
                        cannon = Instantiate<GameObject>(prefabFastCannons[def.cannonLevel]);
                        cannon.transform.SetParent(TANK);
                        cannon.transform.position = TANK.transform.position;
                    }
                }
                break;
        }
        charactersDeterminator();
    }

    public void ItemDroper(Loot loot)
    {
        int l = loot.level;
        string t = loot.type;
        GameObject item;
        switch (t)
        {
            case "hull":
                item = Instantiate<GameObject>(prefabHulls[l]);
                break;
            case "tower":
                item = Instantiate<GameObject>(prefabTowers[l]);
                break;
            default:
                if (def.cannonType == CannonType.FastCannons)
                {
                    item = Instantiate<GameObject>(prefabPowerCannons[l]);
                } 
                else item = Instantiate<GameObject>(prefabFastCannons[l]);
                break;
        }
        item.tag = "Loot";
        item.transform.SetParent(loot.transform);
        Collider col = loot.GetComponent<Collider>();
        col.isTrigger = false;
    }

    public GameObject TargetsFinder(params string[] tags)
    {
        List<GameObject> targets = new List<GameObject>();
        List<float> distanses = new List<float>();
        for (int i = 0; i < tags.Length; i++)
        {
            GameObject[] t;
            string tag = tags[i];
            t = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject target in t)
            {
                targets.Add(target);
            }
        }

        foreach (GameObject target in targets)
        {
            float distanse = Mathf.Abs(target.transform.position.magnitude - transform.position.magnitude);
            distanses.Add(distanse);
        }
        int indexMin = IndexOfMin(distanses);

        GameObject tg = targets[indexMin];

        //// позиция цели
        //Vector3 pPos = target.transform.position;
        //pY = (int)Mathf.Round(pPos.y);
        //pX = (int)Mathf.Round(pPos.x);
        return tg;
    }

    public static int IndexOfMin(List<float> self)// переписать к чертям собачьим
    {
        float min = self[0];
        int minIndex = 0;

        for (int i = 1; i < self.Count; ++i)
        {
            if (self[i] < min)
            {
                min = self[i];
                minIndex = i;
            }
        }

        return minIndex;
    }

    public int pX { get; set; }
    public int pY { get; set; }
    public int rX { get; set; }
    public int rY { get; set; }
}
