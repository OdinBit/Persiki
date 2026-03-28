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
            .To<DamageSystem>()
            .AsSingle();
    }

    private void BindHealthService()
    {
        Container
            .Bind<IHealthService>()
            .To<HealthSystem>()
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
            .To<CombatSystem>()
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
    }
}
