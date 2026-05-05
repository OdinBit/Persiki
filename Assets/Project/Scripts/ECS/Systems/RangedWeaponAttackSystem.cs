using Leopotam.EcsLite;
using UnityEngine;

public class RangedWeaponAttackSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _filter;
    private EcsPool<WeaponAttackCommandComponent> _weaponAttackCommandPool;
    private EcsPool<ItemOwnerComponent> _itemOwnerPool;
    private EcsPool<TransformComponent> _transformPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<WeaponItemComponent>()
            .Inc<RangedWeaponComponent>()
            .Inc<WeaponAttackCommandComponent>()
            .Inc<ItemOwnerComponent>()
            .Inc<TransformComponent>()
            .End();
        _weaponAttackCommandPool = world.GetPool<WeaponAttackCommandComponent>();
        _itemOwnerPool = world.GetPool<ItemOwnerComponent>();
        _transformPool = world.GetPool<TransformComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var weaponEntity in _filter)
        {
            ref var weaponAttackCommand = ref _weaponAttackCommandPool.Get(weaponEntity);
            ref var itemOwnerComponent = ref _itemOwnerPool.Get(weaponEntity);
            ref var transformComponent = ref _transformPool.Get(weaponEntity);

            _weaponAttackCommandPool.Del(weaponEntity);
        }
    }
}
