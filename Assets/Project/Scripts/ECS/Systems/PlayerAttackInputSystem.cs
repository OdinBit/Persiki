using UnityEngine;
using Leopotam.EcsLite;

public class PlayerAttackInputSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
{
    private readonly IMouseAttackInputService _mouseAttackInputService;
    private bool _hasPendingTarget;
    private Vector2 _pendingTarget;
    private EcsFilter _filter;
    private EcsPool<CombatComponent> _combatPool;

    public PlayerAttackInputSystem(IMouseAttackInputService mouseAttackInputService)
    {
        _mouseAttackInputService = mouseAttackInputService;
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<PlayerTag>().Inc<CombatComponent>().End();
        _combatPool = world.GetPool<CombatComponent>();
        _mouseAttackInputService.OnAttackPressed += OnAttackPressed;
    }

    public void Run(IEcsSystems systems)
    {
        if (!_hasPendingTarget) return;

        foreach (var entity in _filter)
        {
            ref var data = ref _combatPool.Get(entity);
            data.TargetPosition = _pendingTarget;
            data.HasTarget = true;
        }

        _hasPendingTarget = false;
    }

    private void OnAttackPressed(Vector3 worldPos)
    {
        _pendingTarget = new Vector2(worldPos.x, worldPos.y);
        _hasPendingTarget = true;
    }

    public void Destroy(IEcsSystems systems)
    {
        _mouseAttackInputService.OnAttackPressed -= OnAttackPressed;
    }
}
