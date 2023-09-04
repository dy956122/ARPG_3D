using System;
using UnityEngine;
public abstract class CharacterProperty : MonoBehaviour
{
    [SerializeField] protected int hp, mp, atk, def, speed;
    protected Rigidbody rigid;
    protected CapsuleCollider selfCollider;
    protected virtual void Attack() { }
    protected virtual void Walk() { }

    // 給系統通知用的
    protected event Action Death;
}