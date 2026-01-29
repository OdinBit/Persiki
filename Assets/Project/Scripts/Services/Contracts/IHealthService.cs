using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthService
{
    public void ApplyHealthChange(IDamageable target, int delta);
}
