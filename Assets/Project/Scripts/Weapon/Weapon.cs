using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;
using Zenject;

public abstract class Weapon : MonoBehaviour
{
    public float Damage;
    protected EventBus _eventBus;
    protected Animator _animator;
    protected DiContainer _container;
    protected IDamageService _damageSystem;

    [Inject]
    public void Construct(EventBus eventBus, DiContainer container, IDamageService damageSystem)
    {
        _container = container;
        _eventBus = eventBus;
        _damageSystem = damageSystem;
    }
    protected void InitWeapon()
    {
        _animator = GetComponentInChildren<Animator>();
        _eventBus.Subscribe<PlayerAttackRequestSignal>(Attack);
    }
    protected virtual void OnDisable()
    {
        if (_eventBus != null) _eventBus.Unsubscribe<PlayerAttackRequestSignal>(Attack);
    }
    public abstract void Attack(PlayerAttackRequestSignal signal);
}
