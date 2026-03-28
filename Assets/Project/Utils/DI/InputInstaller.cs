using UnityEngine;
using Zenject;

public class InputInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindGameInput();
        BindMousePositionHandler();
        BindKeyboardMoveInputHandler();
        BindMouseAttackInputSystem();

    }
    private void BindGameInput()
    {
        Container
            .Bind<GameInput>()
            .AsSingle()
            .NonLazy();
    }
    private void BindMouseAttackInputSystem()
    {
        Container
             .Bind<IMouseAttackInputService>()
             .To<MouseAttackInputSystem>()
             .AsSingle()
             .NonLazy();
    }
    private void BindKeyboardMoveInputHandler()
    {
        Container
            .BindInterfacesTo<MovementInputHandler>()
            .AsSingle()
            .NonLazy();
    }
    private void BindMousePositionHandler()
    {
        Container
            .BindInterfacesAndSelfTo<MousePositionHandler>()
            .AsSingle()
            .NonLazy();
    }
}