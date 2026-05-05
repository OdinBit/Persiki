using Leopotam.EcsLite;
using UnityEngine;

public class MeleeWeaponAttackSystem : IEcsInitSystem, IEcsRunSystem
{

    private EcsFilter _filter;
    private EcsPool<AnimatorComponent> _animatorPool;
    private EcsPool<MeleeWeaponComponent> _meleeWeaponPool;
    private EcsPool<WeaponAttackCommandComponent> _weaponAttackCommandPool;
    private EcsPool<ItemOwnerComponent> _itemOwnerPool;
    private EcsPool<TransformComponent> _transformPool;
    private EcsPool<MeleeWeaponAnimationStateComponent> _meleeWeaponAnimationStatePool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<WeaponItemComponent>()
            .Inc<MeleeWeaponComponent>()
            .Inc<AnimatorComponent>()
            .Inc<WeaponAttackCommandComponent>()
            .Inc<ItemOwnerComponent>()
            .Inc<TransformComponent>()
            .End();
        _animatorPool                   = world.GetPool<AnimatorComponent>();
        _meleeWeaponPool                = world.GetPool<MeleeWeaponComponent>();
        _weaponAttackCommandPool        = world.GetPool<WeaponAttackCommandComponent>();
        _itemOwnerPool                  = world.GetPool<ItemOwnerComponent>();
        _transformPool                  = world.GetPool<TransformComponent>();
        _meleeWeaponAnimationStatePool  = world.GetPool<MeleeWeaponAnimationStateComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var weaponEntity in _filter)
        {
            ref var animatorComponent = ref _animatorPool.Get(weaponEntity);
            ref var weaponAttackCommand = ref _weaponAttackCommandPool.Get(weaponEntity);
            ref var itemOwnerComponent = ref _itemOwnerPool.Get(weaponEntity);
            ref var transformComponent = ref _transformPool.Get(weaponEntity);

            if (animatorComponent.animator != null)
            {
                animatorComponent.animator.Play(MeleeWeaponComponent.AttackAnimationStateName, 0, 0f);
            }

            if (!_meleeWeaponAnimationStatePool.Has(weaponEntity))
            {
                _meleeWeaponAnimationStatePool.Add(weaponEntity);
            }
            _weaponAttackCommandPool.Del(weaponEntity);
        }
    }
}
