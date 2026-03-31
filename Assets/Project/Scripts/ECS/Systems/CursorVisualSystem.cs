using UnityEngine;
using Leopotam.EcsLite;

public class CursorVisualSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _filter;
    private EcsPool<CursorPositionComponent> _posPool;
    private EcsPool<CursorViewComponent> _viewPool;

    public void Init(IEcsSystems systems)
    {
        var world   = systems.GetWorld();
        _filter     = world.Filter<CursorPositionComponent>().Inc<CursorViewComponent>().End();
        _posPool    = world.GetPool<CursorPositionComponent>();
        _viewPool   = world.GetPool<CursorViewComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            ref var pos = ref _posPool.Get(entity);
            ref var view = ref _viewPool.Get(entity);
            if (view.Transform == null) continue;

            Vector3 current = view.Transform.position;
            view.Transform.position = new Vector3(pos.WorldPosition.x, pos.WorldPosition.y, current.z) + view.Offset;
        }
    }
}
