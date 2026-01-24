using CustomEventBus;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindDamageSystem();
        BindEventBus();
        BindHealthSystem();
    }

    private void BindDamageSystem()
    {
        Container
            .Bind<IDamageSystem>()
            .To<DamageSystem>()
            .AsSingle();
    }

    private void BindHealthSystem()
    {
        Container
            .Bind<IHealthSystem>()
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
}