using CustomEventBus;
using CustomEventBus.Signals;
using Leopotam.EcsLite;

public class PlayerAttackSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
{
    private readonly EventBus _eventBus;
    private bool _attackFinished;
    private EcsFilter _filter;
    private EcsPool<CombatComponent> _combatPool;

    public PlayerAttackSystem(EventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<PlayerTag>().Inc<CombatComponent>().End();
        _combatPool = world.GetPool<CombatComponent>();
        _eventBus.Subscribe<PlayerAttackResponseSignal>(OnAttackFinished);
    }

    public void Run(IEcsSystems systems)
    {
        if (_attackFinished)
        {
            foreach (var entity in _filter)
            {
                ref var data = ref _combatPool.Get(entity);
                data.AttackStatus = EAttackStatus.Finished;
                data.HasTarget = false;
            }
            _attackFinished = false;
        }

        foreach (var entity in _filter)
        {
            ref var data = ref _combatPool.Get(entity);
            if (!data.HasTarget) continue;
            if (data.AttackStatus == EAttackStatus.InProgress) continue;

            data.AttackStatus = EAttackStatus.InProgress;
            _eventBus.Invoke(new PlayerAttackRequestSignal(data.TargetPosition));
        }
    }

    private void OnAttackFinished(PlayerAttackResponseSignal signal)
    {
        _attackFinished = true;
    }

    public void Destroy(IEcsSystems systems)
    {
        _eventBus.Unsubscribe<PlayerAttackResponseSignal>(OnAttackFinished);
    }
}
