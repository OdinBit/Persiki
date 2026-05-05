using UnityEngine;
using Leopotam.EcsLite;
public class ItemSetupSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _filter;
    private EcsPool<ItemMarkerComponent> _itemMarkerPool;
    private EcsPool<ItemSpawnedComponent> _itemSpawnedPool;
    private IItemFactoryService _itemFactoryService;

    public ItemSetupSystem(IItemFactoryService itemFactoryService)
    {
        _itemFactoryService = itemFactoryService;
    }

    public void Init(IEcsSystems systems)
    {
        var world       = systems.GetWorld();
        _filter         = world.Filter<ItemMarkerComponent>().Inc<InventoryComponent>().End();
        _itemMarkerPool = world.GetPool<ItemMarkerComponent>();
        _itemSpawnedPool = world.GetPool<ItemSpawnedComponent>();

        _itemFactoryService.Load();

    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            if (_itemSpawnedPool.Has(entity))
            {
                continue;
            }

            ref var itemMarkerComponent = ref _itemMarkerPool.Get(entity);
            if (itemMarkerComponent.marker == null)
            {
                continue;
            }

            _itemFactoryService.Create(itemMarkerComponent.marker, entity);
            _itemSpawnedPool.Add(entity);
        }
    }
}
