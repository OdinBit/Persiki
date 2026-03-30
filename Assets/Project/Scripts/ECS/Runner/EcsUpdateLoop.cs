using UnityEngine;
using Zenject;

public class EcsUpdateLoop : ITickable
{
    private readonly EcsWorld _world;
    private readonly EcsSystemGroup _group;

    public EcsUpdateLoop(
        EcsWorld world,
        PlayerMoveInputSystem moveInputSystem,
        PlayerAttackInputSystem attackInputSystem,
        PlayerAttackSystem attackSystem,
        CharacterAnimationSystem animationSystem)
    {
        _world = world;
        _group = new EcsSystemGroup();
        _group.Add(moveInputSystem);
        _group.Add(attackInputSystem);
        _group.Add(attackSystem);
        _group.Add(animationSystem);
    }

    public void Tick()
    {
        _group.Update(_world, Time.deltaTime);
    }
}
