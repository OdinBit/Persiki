using System;
using Leopotam.EcsLite;
using Zenject;

public class EcsStartup : IInitializable, ITickable, IFixedTickable, IDisposable
{
    private readonly EcsWorld _world;
    private readonly IKeyboardMoveInputService _keyboardMoveInputService;
    private readonly IMouseAttackInputService _mouseAttackInputService;
    private readonly IMousePositionService _mousePositionService;
    private readonly IMovementService _movementService;
    private readonly IItemFactoryService _itemFactoryService;
    private readonly CustomEventBus.EventBus _eventBus;

    private IEcsSystems _updateSystems;
    private IEcsSystems _fixedSystems;

    public EcsStartup(
        EcsWorld world,
        IKeyboardMoveInputService keyboardMoveInputService,
        IMouseAttackInputService mouseAttackInputService,
        IMousePositionService mousePositionService,
        IMovementService movementService,
        IItemFactoryService itemFactoryService,
        CustomEventBus.EventBus eventBus)
    {
        _world                      = world;
        _keyboardMoveInputService   = keyboardMoveInputService;
        _mouseAttackInputService    = mouseAttackInputService;
        _mousePositionService       = mousePositionService;
        _movementService            = movementService;
        _eventBus                   = eventBus;
        _itemFactoryService         = itemFactoryService;
    }

    public void Initialize()
    {
        _updateSystems = new EcsSystems(_world)
            .Add(new PlayerMoveInputSystem(_keyboardMoveInputService))
            .Add(new PlayerAttackInputSystem(_mouseAttackInputService))
            .Add(new PlayerAttackSystem(_eventBus))
            .Add(new CharacterAnimationSystem())
            .Add(new CursorInputSystem(_mousePositionService))
            .Add(new CursorVisualSystem())
            .Add(new ItemSetupSystem(_itemFactoryService));

        _updateSystems.Init();

        _fixedSystems = new EcsSystems(_world)
            .Add(new PlayerMovementSystem(_movementService));

        _fixedSystems.Init();
    }

    public void Tick()
    {
        _updateSystems?.Run();
    }

    public void FixedTick()
    {
        _fixedSystems?.Run();
    }

    public void Dispose()
    {
        _fixedSystems?.Destroy();
        _updateSystems?.Destroy();
        _world?.Destroy();
    }
}
