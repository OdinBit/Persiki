using UnityEngine;
using Zenject;
using Leopotam.EcsLite;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    private EcsWorld _world;
    private int _entity;
    private Rigidbody2D _rb2d;
    private Animator _animator;
    private ItemMarker _itemMarker;

    [Inject]
    public void Construct(EcsWorld world)
    {
        _world = world;
    }

    private void Awake()
    {
        _rb2d       = GetComponent<Rigidbody2D>();
        _animator   = GetComponent<Animator>();
        _itemMarker = GetComponentInChildren<ItemMarker>();
    }

    private void Start()
    {
        if (_world == null) return;

        _entity = _world.NewEntity();

        _world.GetPool<PlayerTag>().Add(_entity);

        ref var rbComp          = ref _world.GetPool<Rigidbody2DComponent>().Add(_entity);
        rbComp.Value            = _rb2d;

        ref var move            = ref _world.GetPool<MovementComponent>().Add(_entity);
        move.Direction          = Vector2.zero;
        move.IsMoving           = false;

        ref var combat          = ref _world.GetPool<CombatComponent>().Add(_entity);
        combat.AttackStatus     = EAttackStatus.Finished;
        combat.HasTarget        = false;

        ref var anim            = ref _world.GetPool<AnimatorComponent>().Add(_entity);
        anim.animator           = _animator;

        ref var moveAnim        = ref _world.GetPool<MoveAnimationComponent>().Add(_entity);
        moveAnim.IsRunning      = false;
        moveAnim.IsFacingRight  = true;

        ref var inventory       = ref _world.GetPool<InventoryComponent>().Add(_entity);

        ref var itemMarker      = ref _world.GetPool<ItemMarkerComponent>().Add(_entity);
        itemMarker.marker       = _itemMarker;

        if (_itemMarker != null)
        {
            ref var itemMarkerFacing = ref _world.GetPool<ItemMarkerFacingComponent>().Add(_entity);
            itemMarkerFacing.BaseLocalPosition = _itemMarker.transform.localPosition;
            itemMarkerFacing.BaseLocalScale = _itemMarker.transform.localScale;
        }
    }

    private void OnDestroy()
    {
        if (_world != null && _world.IsAlive() && _entity >= 0)
        {
            _world.DelEntity(_entity);
        }
    }

    public Vector2 GetPlayerPosition()
    {
        return (Vector2)transform.position;
    }
}
