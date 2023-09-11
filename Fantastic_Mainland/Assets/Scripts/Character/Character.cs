using UnityEngine;

public class Character : CharacterProperty
{
    protected override void Attack()
    {
        print("Attack");
    }

    protected override void Walk()
    {
        print("walk");
    }

    void Awake()
    {
        // Attack();
        // Walk();
    }

    void Start()
    {
        // hp = 15;
        // mp = 30;
        // atk = 10;
        // def = 16;
        // speed = 23;
        // rigid = GetComponent<Rigidbody>();
        // selfCollider = GetComponent<CapsuleCollider>();

    }
}