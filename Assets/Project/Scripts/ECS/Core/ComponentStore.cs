using System.Collections.Generic;

public interface IComponentStore
{
    bool Has(int entityId);
    void Remove(int entityId);
    int Count { get; }
    IEnumerable<int> EntityIds { get; }
}

public class ComponentStore<T> : IComponentStore
{
    private readonly Dictionary<int, T> _items = new Dictionary<int, T>();

    public bool Has(int entityId) => _items.ContainsKey(entityId);

    public void Remove(int entityId) => _items.Remove(entityId);

    public int Count => _items.Count;

    public IEnumerable<int> EntityIds => _items.Keys;

    public void Set(int entityId, T component)
    {
        _items[entityId] = component;
    }

    public T Get(int entityId)
    {
        return _items[entityId];
    }

    public bool TryGet(int entityId, out T component)
    {
        return _items.TryGetValue(entityId, out component);
    }
}
