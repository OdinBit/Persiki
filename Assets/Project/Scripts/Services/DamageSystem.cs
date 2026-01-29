using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : IDamageService
{
    readonly IHealthService _healthSystem;
    public DamageSystem(IHealthService healthSystem)
    {
        _healthSystem = healthSystem;
    }
    /// <summary>
    /// Applies damage to the given target by calculating the effective damage value
    /// and then passing it to the HealthSystem to update the target's health.
    /// </summary>
    public void DealDamage(IDamageable target, DamageInfo info)
    {
        int calculatedDamage = DamageCalculate(info);
        _healthSystem.ApplyHealthChange(target, -calculatedDamage);
    }

    /// <summary>
    /// Calculates the base damage amount based on the provided DamageInfo.
    /// 
    /// NOTE: This is currently a simple computation that just returns the raw damage value.
    /// In the future, this method can be expanded to support more complex calculations such as:
    /// - armor and resistance modifiers
    /// - critical hits and critical damage multipliers
    /// - elemental damage types and their interactions
    /// - damage reduction effects (shields, buffs, debuffs)
    /// 
    /// All such logic should be encapsulated here to keep the damage calculation centralized
    /// and easily maintainable.
    /// </summary>
    private int DamageCalculate(DamageInfo info)
    {
        return info.damage;
    }

}
