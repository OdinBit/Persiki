using UnityEngine;
using Zenject;
using Leopotam.EcsLite;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EdgeCollider2D))]
public class SimpleSword : MonoBehaviour
{
    private EcsWorld _world;
    private int _entity = -1;
    private Animator _animator;
    private Transform _transform;
    private int _ownerEntity = -1;

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

    public void SetOwnerEntity(int ownerEntity)
    {
        _ownerEntity = ownerEntity;
    }

    private void Start()
    {
        if (_world == null) return;

        _entity = _world.NewEntity();

        ref var animComponent = ref _world.GetPool<AnimatorComponent>().Add(_entity);
        animComponent.animator = _animator;

        ref var transformComponent = ref _world.GetPool<TransformComponent>().Add(_entity);
        transformComponent.Transform = _transform;

        ref var itemOwnerComponent = ref _world.GetPool<ItemOwnerComponent>().Add(_entity);
        itemOwnerComponent.OwnerEntity = _ownerEntity;

        _world.GetPool<WeaponItemComponent>().Add(_entity);
    }

    private void OnDestroy()
    {
        if (_world != null && _world.IsAlive() && _entity >= 0)
        {
            _world.DelEntity(_entity);
        }
    }
}
