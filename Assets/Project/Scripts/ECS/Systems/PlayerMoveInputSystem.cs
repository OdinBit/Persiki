using UnityEngine;
using Leopotam.EcsLite;

public class PlayerMoveInputSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly IKeyboardMoveInputService _inputService;
    private EcsFilter _filter;
    private EcsPool<MovementComponent> _movePool;

    public PlayerMoveInputSystem(IKeyboardMoveInputService inputService)
    {
        _inputService = inputService;
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<PlayerTag>().Inc<MovementComponent>().End();
        _movePool = world.GetPool<MovementComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        Vector2 direction = _inputService.GetMoveDirection();
        bool isMoving = direction.sqrMagnitude > 0.01f;

        foreach (var entity in _filter)
        {
            ref var data = ref _movePool.Get(entity);
            data.Direction = direction;
            data.IsMoving = isMoving;
        }
    }
}
