using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class Enemy : MonoBehaviour, 
                     IHealthHolder, 
                     IDamageable
{
    [SerializeField] private int _health = 3;
    [SerializeField] private Color _hitColor = Color.red;
    [SerializeField] private float _flashTime = 0.1f;

    private SpriteRenderer _spriteRenderer;
    private EnemyMovement _enemyMovement;
    private Color _defaultColor;

    public int HealthPoints { get; set; }

    [Inject]
    public void Construct()
    {

    }

    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyMovement.Init();

        _defaultColor = _spriteRenderer.color;
    }
    private void Update()
    {

    }

    public void OnDamage()
    {
        _enemyMovement.Knockback();

        StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        _spriteRenderer.color = _hitColor;
        yield return new WaitForSeconds(_flashTime);
        _spriteRenderer.color = _defaultColor;
    }
}
