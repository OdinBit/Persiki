using UnityEngine;
using Leopotam.EcsLite;

public class CharacterAnimationSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _filter;
    private EcsPool<MovementComponent> _movePool;
    private EcsPool<AnimatorComponent> _animPool;
    private EcsPool<MoveAnimationComponent> _animStatePool;

    public void Init(IEcsSystems systems)
    {
        var world       = systems.GetWorld();
        _filter         = world.Filter<AnimatorComponent>().Inc<MovementComponent>().End();
        _movePool       = world.GetPool<MovementComponent>();
        _animPool       = world.GetPool<AnimatorComponent>();
        _animStatePool  = world.GetPool<MoveAnimationComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            ref var movementData = ref _movePool.Get(entity);
            ref var animatorData = ref _animPool.Get(entity);

            if (animatorData.animator == null) continue;

            bool hasAnimState = _animStatePool.Has(entity);
            ref var moveAnim = ref (hasAnimState
                ? ref _animStatePool.Get(entity)
                : ref _animStatePool.Add(entity));

            if (!hasAnimState)
            {
                moveAnim.IsRunning = false;
                moveAnim.IsFacingRight = true;
            }

            bool shouldRun = movementData.IsMoving;
            float dirX = movementData.Direction.x;
            bool hasHorizontalInput = Mathf.Abs(dirX) > 0.01f;

            if (hasHorizontalInput)
            {
                moveAnim.IsFacingRight = dirX > 0f;
            }

            string targetState = shouldRun
                ? (moveAnim.IsFacingRight ? "RUN_RIGHT" : "RUN_LEFT")
                : (moveAnim.IsFacingRight ? "IDLE_RIGHT" : "IDLE_LEFT");

            if (!animatorData.animator.GetCurrentAnimatorStateInfo(0).IsName(targetState))
            {
                animatorData.animator.Play(targetState, 0);
            }

            moveAnim.IsRunning = shouldRun;
        }
    }
}
