using UnityEngine;
using Zenject;
using Leopotam.EcsLite;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EdgeCollider2D))]
public class SimpleSword : MonoBehaviour
{
    private EcsWorld _world;
    private int _entity;
    private Animator _animator;
    private Transform _transform;

    [Inject]
    public void Construct(EcsWorld world)
    {
        _world = world;
    }

    private void Awake()
    {
        _transform  = GetComponent<Transform>();
        _animator   = GetComponent<Animator>();
    }
    private void Start()
    {
        if (_world == null) return;

        _entity = _world.NewEntity();

        ref var animComponent = ref _world.GetPool<AnimatorComponent>().Add(_entity);
        animComponent.animator = _animator;

        ref var inputAttackEventComponent = ref _world.GetPool<InputAttackEventComponent>().Add(_entity);

        ref var transformComponent = ref _world.GetPool<TransformComponent>().Add(_entity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
