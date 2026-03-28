using System;
using CustomEventBus;
using CustomEventBus.Signals;

public class PlayerAttackSystem : IEcsSystem, System.IDisposable
{
    private readonly EventBus _eventBus;
    private bool _attackFinished;

    public PlayerAttackSystem(EventBus eventBus)
    {
        _eventBus = eventBus;
        _eventBus.Subscribe<PlayerAttackResponseSignal>(OnAttackFinished);
    }

    public void Update(EcsWorld world, float deltaTime)
    {
        if (_attackFinished)
        {
            foreach (var entity in world.Query<PlayerTag, PlayerAttackData>())
            {
                var data = world.Get<PlayerAttackData>(entity);
                data.AttackStatus = EAttackStatus.Finished;
                data.HasTarget = false;
                world.Set(entity, data);
            }
            _attackFinished = false;
        }

        foreach (var entity in world.Query<PlayerTag, PlayerAttackData>())
        {
            var data = world.Get<PlayerAttackData>(entity);
            if (!data.HasTarget) continue;
            if (data.AttackStatus == EAttackStatus.InProgress) continue;

            data.AttackStatus = EAttackStatus.InProgress;
            world.Set(entity, data);
            _eventBus.Invoke(new PlayerAttackRequestSignal(data.TargetPosition));
        }
    }

    private void OnAttackFinished(PlayerAttackResponseSignal signal)
    {
        _attackFinished = true;
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<PlayerAttackResponseSignal>(OnAttackFinished);
    }
}
