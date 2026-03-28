using System.Collections.Generic;

public class EcsSystemGroup
{
    private readonly List<IEcsSystem> _systems = new List<IEcsSystem>();

    public void Add(IEcsSystem system)
    {
        if (system == null) return;
        _systems.Add(system);
    }

    public void Update(EcsWorld world, float deltaTime)
    {
        for (int i = 0; i < _systems.Count; i++)
        {
            _systems[i].Update(world, deltaTime);
        }
    }
}
