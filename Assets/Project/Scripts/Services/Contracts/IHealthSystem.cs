using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthSystem
{
    public void ApplyHealthChange(IDamageable target, int delta);
}
