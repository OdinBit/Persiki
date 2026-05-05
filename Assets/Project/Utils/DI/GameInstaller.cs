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
        BindItemFactory();
    }

    private void BindItemFactory()
    {
        Container
            .Bind<IItemFactoryService>()
            .To<ItemFactory>()
            .AsSingle();
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
            .Bind<Leopotam.EcsLite.EcsWorld>()
            .AsSingle();

        Container
            .BindInterfacesTo<EcsStartup>()
            .AsSingle();
    }
}
