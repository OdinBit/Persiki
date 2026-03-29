using System;
using UnityEngine;

public class PlayerAttackInputSystem : IEcsSystem, IDisposable
{
    private readonly IMouseAttackInputService _mouseAttackInputService;
    private bool _hasPendingTarget;
    private Vector2 _pendingTarget;

    public PlayerAttackInputSystem(IMouseAttackInputService mouseAttackInputService)
    {
        _mouseAttackInputService = mouseAttackInputService;
        _mouseAttackInputService.OnAttackPressed += OnAttackPressed;
    }

    public void Update(EcsWorld world, float deltaTime)
    {
        if (!_hasPendingTarget) return;

        foreach (var entity in world.Query<PlayerTag, CombatComponent>())
        {
            var data = world.Get<CombatComponent>(entity);
            data.TargetPosition = _pendingTarget;
            data.HasTarget = true;
            world.Set(entity, data);
        }

        _hasPendingTarget = false;
    }

    private void OnAttackPressed(Vector3 worldPos)
    {
        _pendingTarget = new Vector2(worldPos.x, worldPos.y);
        _hasPendingTarget = true;
    }

    public void Dispose()
    {
        _mouseAttackInputService.OnAttackPressed -= OnAttackPressed;
    }
}
