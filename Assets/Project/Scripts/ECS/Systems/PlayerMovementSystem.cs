using UnityEngine;

public class PlayerMovementSystem : IEcsSystem
{
    private readonly IMovementService _movementService;

    public PlayerMovementSystem(IMovementService movementService)
    {
        _movementService = movementService;
    }

    public void Update(EcsWorld world, float deltaTime)
    {
        foreach (var entity in world.Query<PlayerTag, Rigidbody2DComponent>())
        {
            if (!world.TryGet(entity, out PlayerMovementData moveData)) continue;

            Vector2 direction = moveData.IsMoving ? moveData.Direction : Vector2.zero;
            var rb = world.Get<Rigidbody2DComponent>(entity).Value;
            _movementService.MoveToDirection(rb, direction);
        }
    }
}
