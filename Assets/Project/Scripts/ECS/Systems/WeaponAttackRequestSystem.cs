using Leopotam.EcsLite;
using UnityEngine;

public class WeaponAttackRequestSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _ownerFilter;
    private EcsFilter _weaponFilter;
    private EcsPool<AttackRequestComponent> _attackRequestPool;
    private EcsPool<ItemOwnerComponent> _itemOwnerPool;
    private EcsPool<TransformComponent> _transformPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _ownerFilter = world.Filter<InventoryComponent>().Inc<AttackRequestComponent>().End();
        _weaponFilter = world.Filter<WeaponItemComponent>().Inc<ItemOwnerComponent>().Inc<TransformComponent>().End();
        _attackRequestPool = world.GetPool<AttackRequestComponent>();
        _itemOwnerPool = world.GetPool<ItemOwnerComponent>();
        _transformPool = world.GetPool<TransformComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var ownerEntity in _ownerFilter)
        {
            ref var attackRequest = ref _attackRequestPool.Get(ownerEntity);

            foreach (var weaponEntity in _weaponFilter)
            {
                ref var itemOwner = ref _itemOwnerPool.Get(weaponEntity);
                if (itemOwner.OwnerEntity != ownerEntity)
                {
                    continue;
                }

                Debug.Log(
                    $"Attack request received. OwnerEntity={ownerEntity}, WeaponEntity={weaponEntity}, Target={attackRequest.TargetPosition}");
            }

            _attackRequestPool.Del(ownerEntity);
        }
    }
}
