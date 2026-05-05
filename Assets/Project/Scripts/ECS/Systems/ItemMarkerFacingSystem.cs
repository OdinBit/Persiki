using Leopotam.EcsLite;
using UnityEngine;

public class ItemMarkerFacingSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _filter;
    private EcsPool<ItemMarkerComponent> _itemMarkerPool;
    private EcsPool<ItemMarkerFacingComponent> _itemMarkerFacingPool;
    private EcsPool<MoveAnimationComponent> _moveAnimationPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<ItemMarkerComponent>()
            .Inc<ItemMarkerFacingComponent>()
            .Inc<MoveAnimationComponent>()
            .End();
        _itemMarkerPool = world.GetPool<ItemMarkerComponent>();
        _itemMarkerFacingPool = world.GetPool<ItemMarkerFacingComponent>();
        _moveAnimationPool = world.GetPool<MoveAnimationComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            ref var itemMarkerComponent = ref _itemMarkerPool.Get(entity);
            if (itemMarkerComponent.marker == null)
            {
                continue;
            }

            var markerTransform = itemMarkerComponent.marker.transform;
            ref var markerFacing = ref _itemMarkerFacingPool.Get(entity);
            ref var moveAnimation = ref _moveAnimationPool.Get(entity);

            var localPosition = markerFacing.BaseLocalPosition;
            localPosition.x = Mathf.Abs(localPosition.x) * (moveAnimation.IsFacingRight ? 1f : -1f);
            markerTransform.localPosition = localPosition;

            var localScale = markerFacing.BaseLocalScale;
            localScale.x = Mathf.Abs(localScale.x) * (moveAnimation.IsFacingRight ? 1f : -1f);
            markerTransform.localScale = localScale;
        }
    }
}
