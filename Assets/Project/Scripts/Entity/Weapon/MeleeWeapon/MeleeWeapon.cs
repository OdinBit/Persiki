using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventBus.Signals;

public class MeleeWeapon : Weapon
{
    private void Start()
    {
        InitWeapon();
    }

    private void SetAttackDirection(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - (Vector2)transform.position;

        float baseAngle = -45f;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                transform.parent.rotation = Quaternion.Euler(0, 0, baseAngle);

            }
            else
            {
                transform.parent.rotation = Quaternion.Euler(0, 180, baseAngle);

            }
        }
        else
        {
            if (direction.y > 0)
            {
                transform.parent.rotation = Quaternion.Euler(0, 180, 90f + baseAngle);
            }
            else
            {
                transform.parent.rotation = Quaternion.Euler(0, 0, -90f + baseAngle);
            }
        }

    }
    public override void Attack(PlayerAttackRequestSignal signal)
    {
        SetAttackDirection(signal.targetPosition);
        _animator.SetTrigger("AttackEvent");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out var target))
        {
            DamageInfo damageInfo = new DamageInfo()
            {
                damage = 0,
                damageType = 0,
                attacker = this.gameObject
            };
            _damageSystem.DealDamage(target, damageInfo);
        }
    }
}
