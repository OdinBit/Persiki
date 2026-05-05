using Leopotam.EcsLite;

public class WeaponAttackRequestSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _ownerFilter;
    private EcsFilter _weaponFilter;
    private EcsPool<AttackRequestComponent> _attackRequestPool;
    private EcsPool<ItemOwnerComponent> _itemOwnerPool;
    private EcsPool<WeaponAttackCommandComponent> _weaponAttackCommandPool;

    public void Init(IEcsSystems systems)
    {
        var world                   = systems.GetWorld();
        _ownerFilter                = world.Filter<InventoryComponent>().Inc<AttackRequestComponent>().End();
        _weaponFilter               = world.Filter<WeaponItemComponent>().Inc<ItemOwnerComponent>().End();
        _attackRequestPool          = world.GetPool<AttackRequestComponent>();
        _itemOwnerPool              = world.GetPool<ItemOwnerComponent>();
        _weaponAttackCommandPool    = world.GetPool<WeaponAttackCommandComponent>();
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

                if (!_weaponAttackCommandPool.Has(weaponEntity))
                {
                    _weaponAttackCommandPool.Add(weaponEntity);
                }

                ref var weaponAttackCommand = ref _weaponAttackCommandPool.Get(weaponEntity);
                weaponAttackCommand.TargetPosition = attackRequest.TargetPosition;
            }

            _attackRequestPool.Del(ownerEntity);
        }
    }
}
