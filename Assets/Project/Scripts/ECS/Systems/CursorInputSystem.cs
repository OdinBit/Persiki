using UnityEngine;
using Leopotam.EcsLite;

public class CursorInputSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly IMousePositionService _mousePositionService;
    private EcsFilter _filter;
    private EcsPool<CursorPositionComponent> _posPool;

    public CursorInputSystem(IMousePositionService mousePositionService)
    {
        _mousePositionService = mousePositionService;
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<CursorPositionComponent>().End();
        _posPool = world.GetPool<CursorPositionComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        Vector2 worldPos = _mousePositionService.GetCursorePosition();

        foreach (var entity in _filter)
        {
            ref var data = ref _posPool.Get(entity);
            data.WorldPosition = worldPos;
        }
    }
}
