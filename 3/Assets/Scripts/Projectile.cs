using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;    
    public float rangeAttack;
    public Vector3 startPosition;
    public Vector3 endPosition;

    void Awake()
    {
        rangeAttack = RangeAttack;
        damage = Damage;
        endPosition = EndPosition;
    }

    void Update()
    {
        if (Mathf.Abs(startPosition.magnitude - transform.position.magnitude) > rangeAttack) Destroy(this.gameObject);//���� ������ �������
    }

    void OnCollisionEnter(Collision coll)
    {           
        Destroy(this.gameObject);    // ��������� �������� �� ����� ��� ������������ � ��������� �����
    }

    public static int Damage { get; set; }
    public static float RangeAttack;
    public static Vector3 EndPosition;
}
