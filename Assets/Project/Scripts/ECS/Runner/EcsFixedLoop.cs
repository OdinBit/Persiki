using UnityEngine;
using Zenject;

public class EcsFixedLoop : IFixedTickable
{
    private readonly EcsWorld _world;
    private readonly EcsSystemGroup _group;

    public EcsFixedLoop(
        EcsWorld world,
        PlayerMovementSystem movementSystem)
    {
        _world = world;
        _group = new EcsSystemGroup();
        _group.Add(movementSystem);
    }

    public void FixedTick()
    {
        _group.Update(_world, Time.fixedDeltaTime);
    }
}
