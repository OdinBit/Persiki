using Zenject;
using UnityEngine;
using System;

public class HealthSystem : IHealthService
{
    public HealthSystem()
    {
    }

    public void ApplyHealthChange(IDamageable target, int delta)
    {
        IHealthHolder healthComponent = (IHealthHolder) target;
        if (healthComponent == null) return;

        if(healthComponent.HealthPoints != 0) healthComponent.HealthPoints += delta;
        healthComponent.OnDamage();
    }
}