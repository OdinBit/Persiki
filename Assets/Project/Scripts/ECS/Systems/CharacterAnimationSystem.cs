using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationSystem : IEcsSystem
{
    public void Update(EcsWorld world, float deltaTime)
    {

        foreach (var entity in world.Query<AnimatorComponent, MovementComponent>())
        {
            var movement_data = world.Get<MovementComponent>(entity);
            var animation_data = world.Get<AnimatorComponent>(entity);

            if (animation_data.animator == null) continue;

            if (!world.TryGet(entity, out MoveAnimationComponent moveAnim))
            {
                moveAnim = new MoveAnimationComponent { IsRunning = false, IsFacingRight = true };
            }

            bool shouldRun = movement_data.IsMoving;
            float dirX = movement_data.Direction.x;
            bool hasHorizontalInput = Mathf.Abs(dirX) > 0.01f;

            if (hasHorizontalInput)
            {
                moveAnim.IsFacingRight = dirX > 0f;
            }

            string targetState = shouldRun
                ? (moveAnim.IsFacingRight ? "RUN_RIGHT" : "RUN_LEFT")
                : (moveAnim.IsFacingRight ? "IDLE_RIGHT" : "IDLE_LEFT");

            if (!animation_data.animator.GetCurrentAnimatorStateInfo(0).IsName(targetState))
            {
                animation_data.animator.Play(targetState, 0);
            }

            if (moveAnim.IsRunning != shouldRun)
            {
                moveAnim.IsRunning = shouldRun;
            }

            world.Set(entity, moveAnim);
        }
    }
}
