using UnityEngine;

public class PlayerMoveInputSystem : IEcsSystem
{
    private readonly IKeyboardMoveInputService _inputService;

    public PlayerMoveInputSystem(IKeyboardMoveInputService inputService)
    {
        _inputService = inputService;
    }

    public void Update(EcsWorld world, float deltaTime)
    {
        Vector2 direction = _inputService.GetMoveDirection();
        bool isMoving = direction.sqrMagnitude > 0.01f;

        foreach (var entity in world.Query<PlayerTag, PlayerMovementData>())
        {
            var data = world.Get<PlayerMovementData>(entity);
            data.Direction = direction;
            data.IsMoving = isMoving;
            world.Set(entity, data);
        }
    }
}
