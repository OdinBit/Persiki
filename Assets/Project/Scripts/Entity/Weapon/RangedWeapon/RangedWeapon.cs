using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RangedWeapon : Weapon
{
    [SerializeField] private GameObject _projectile;
    private Vector2 targetPosition;

    private void InitRangedWeapon()
    {
        _eventBus.Subscribe<RenderRangedWeaponFinishAttackSignal>(AttackFinished);
    }
    public void Start()
    {
        InitWeapon();
        InitRangedWeapon();
    }
    public override void Attack(PlayerAttackRequestSignal signal)
    {
        targetPosition = signal.targetPosition;
        _eventBus.Invoke(new RenderRangedWeaponPlayAttackSignal());
        targetPosition = signal.targetPosition;
        Vector3 spawnPos = transform.position;
        Vector2 direction = (targetPosition - (Vector2)spawnPos).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //angle -= 90f; 
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = rotation;
    }

    public void AttackFinished(RenderRangedWeaponFinishAttackSignal signal)
    {

        Vector3 spawnPos = transform.position;

        Vector2 direction = (targetPosition - (Vector2)spawnPos).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= 90;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        GameObject arrowInstance = _container.InstantiatePrefab(_projectile, spawnPos, rotation, null);

        Rigidbody2D rb = arrowInstance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * 30f;
        }
        Destroy(arrowInstance, 10f);
        _eventBus.Invoke(new PlayerAttackResponseSignal());
    }
}
