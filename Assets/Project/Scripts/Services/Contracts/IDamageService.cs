using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo
{
    public int damage;
    public int damageType;
    public GameObject attacker;
}
public interface IDamageService
{
    void DealDamage(IDamageable targe, DamageInfo info);
}
