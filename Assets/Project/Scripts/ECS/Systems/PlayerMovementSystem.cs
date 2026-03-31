using UnityEngine;
using Leopotam.EcsLite;

public class PlayerMovementSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly IMovementService _movementService;
    private EcsFilter _filter;
    private EcsPool<Rigidbody2DComponent> _rbPool;
    private EcsPool<MovementComponent> _movePool;

    public PlayerMovementSystem(IMovementService movementService)
    {
        _movementService = movementService;
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<PlayerTag>().Inc<Rigidbody2DComponent>().Inc<MovementComponent>().End();
        _rbPool = world.GetPool<Rigidbody2DComponent>();
        _movePool = world.GetPool<MovementComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            ref var moveData = ref _movePool.Get(entity);
            Vector2 direction = moveData.IsMoving ? moveData.Direction : Vector2.zero;
            ref var rbComp = ref _rbPool.Get(entity);
            _movementService.MoveToDirection(rbComp.Value, direction);
        }
    }
}
