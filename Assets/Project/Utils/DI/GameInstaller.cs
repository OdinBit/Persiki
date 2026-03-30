using CustomEventBus;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindDamageService();
        BindEventBus();
        BindHealthService();
        BindCombatService();
        BindEcs();
    }

    private void BindDamageService()
    {
        Container
            .Bind<IDamageService>()
            .To<DamageService>()
            .AsSingle();
    }

    private void BindHealthService()
    {
        Container
            .Bind<IHealthService>()
            .To<HealthService>()
            .AsSingle();
    }

    private void BindEventBus()
    {
        Container
            .Bind<EventBus>()
            .FromNew()
            .AsSingle();
    }

    private void BindCombatService()
    {
        Container
            .Bind<ICombatService>()
            .To<CombatService>()
            .AsTransient();
    }

    private void BindEcs()
    {
        Container
            .Bind<EcsWorld>()
            .AsSingle();

        Container
            .Bind<PlayerMoveInputSystem>()
            .AsSingle();

        Container
            .Bind<PlayerMovementSystem>()
            .AsSingle();

        Container
            .Bind<PlayerAttackInputSystem>()
            .AsSingle();

        Container
            .Bind<PlayerAttackSystem>()
            .AsSingle();

        Container
            .BindInterfacesTo<EcsUpdateLoop>()
            .AsSingle();

        Container
            .BindInterfacesTo<EcsFixedLoop>()
            .AsSingle();

        Container
            .Bind<CharacterAnimationSystem>()
            .AsSingle();
    }
}
