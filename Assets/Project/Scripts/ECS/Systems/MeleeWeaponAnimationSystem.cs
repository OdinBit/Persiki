using Leopotam.EcsLite;

public class MeleeWeaponAnimationSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _filter;
    private EcsPool<AnimatorComponent> _animatorPool;
    private EcsPool<MeleeWeaponComponent> _meleeWeaponPool;
    private EcsPool<MeleeWeaponAnimationStateComponent> _meleeWeaponAnimationStatePool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<WeaponItemComponent>()
            .Inc<MeleeWeaponComponent>()
            .Inc<AnimatorComponent>()
            .Inc<MeleeWeaponAnimationStateComponent>()
            .End();
        _animatorPool = world.GetPool<AnimatorComponent>();
        _meleeWeaponPool = world.GetPool<MeleeWeaponComponent>();
        _meleeWeaponAnimationStatePool = world.GetPool<MeleeWeaponAnimationStateComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var weaponEntity in _filter)
        {
            ref var animatorComponent = ref _animatorPool.Get(weaponEntity);

            if (animatorComponent.animator == null)
            {
                _meleeWeaponAnimationStatePool.Del(weaponEntity);
                continue;
            }

            var stateInfo = animatorComponent.animator.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName(MeleeWeaponComponent.AttackAnimationStateName))
            {
                continue;
            }

            if (stateInfo.normalizedTime < 1f)
            {
                continue;
            }

            animatorComponent.animator.Play(MeleeWeaponComponent.IdleAnimationStateName, 0, 0f);
            _meleeWeaponAnimationStatePool.Del(weaponEntity);
        }
    }
}
