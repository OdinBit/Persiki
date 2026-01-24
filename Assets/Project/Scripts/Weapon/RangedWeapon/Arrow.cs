using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;
using Zenject;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    private IDamageSystem   _damageSystem;

    [Inject]
    public void Construct(IDamageSystem damageSystem)
    {
        _damageSystem   = damageSystem;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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
