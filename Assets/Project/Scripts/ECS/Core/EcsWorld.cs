using System;
using System.Collections.Generic;

public class EcsWorld
{
    private readonly Dictionary<Type, IComponentStore> _stores = new Dictionary<Type, IComponentStore>();
    private readonly HashSet<int> _alive = new HashSet<int>();
    private int _nextEntityId;

    public EcsEntity CreateEntity()
    {
        int id = _nextEntityId++;
        _alive.Add(id);
        return new EcsEntity(id);
    }

    public void DestroyEntity(EcsEntity entity)
    {
        if (!_alive.Remove(entity.Id)) return;

        foreach (var store in _stores.Values)
        {
            store.Remove(entity.Id);
        }
    }

    public bool IsAlive(EcsEntity entity) => _alive.Contains(entity.Id);

    public void Add<T>(EcsEntity entity, T component)
    {
        GetOrCreateStore<T>().Set(entity.Id, component);
    }

    public void Set<T>(EcsEntity entity, T component)
    {
        GetOrCreateStore<T>().Set(entity.Id, component);
    }

    public bool Has<T>(EcsEntity entity)
    {
        var store = GetStore<T>();
        return store != null && store.Has(entity.Id);
    }

    public bool TryGet<T>(EcsEntity entity, out T component)
    {
        var store = GetStore<T>();
        if (store == null)
        {
            component = default;
            return false;
        }
        return store.TryGet(entity.Id, out component);
    }

    public T Get<T>(EcsEntity entity)
    {
        var store = GetStore<T>();
        if (store == null) throw new InvalidOperationException("Component store not found: " + typeof(T).Name);
        return store.Get(entity.Id);
    }

    public void Remove<T>(EcsEntity entity)
    {
        var store = GetStore<T>();
        store?.Remove(entity.Id);
    }

    public IEnumerable<EcsEntity> Query<T>()
    {
        var store = GetStore<T>();
        if (store == null) yield break;

        foreach (var id in store.EntityIds)
        {
            if (_alive.Contains(id))
            {
                yield return new EcsEntity(id);
            }
        }
    }

    public IEnumerable<EcsEntity> Query<T1, T2>()
    {
        var store1 = GetStore<T1>();
        var store2 = GetStore<T2>();
        if (store1 == null || store2 == null) yield break;

        IComponentStore store1Base = store1;
        IComponentStore store2Base = store2;

        var primary = store1Base.Count <= store2Base.Count ? store1Base : store2Base;
        var otherType = ReferenceEquals(primary, store1Base) ? typeof(T2) : typeof(T1);

        foreach (var id in primary.EntityIds)
        {
            if (!_alive.Contains(id)) continue;
            if (Has(otherType, id)) yield return new EcsEntity(id);
        }
    }

    private bool Has(Type type, int entityId)
    {
        if (!_stores.TryGetValue(type, out var store)) return false;
        return store.Has(entityId);
    }

    private ComponentStore<T> GetStore<T>()
    {
        if (_stores.TryGetValue(typeof(T), out var store))
        {
            return (ComponentStore<T>)store;
        }
        return null;
    }

    private ComponentStore<T> GetOrCreateStore<T>()
    {
        var store = GetStore<T>();
        if (store != null) return store;

        var newStore = new ComponentStore<T>();
        _stores[typeof(T)] = newStore;
        return newStore;
    }
}
