using UnityEngine;
using Leopotam.EcsLite;
public class ItemSetupSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _filter;
    private EcsPool<ItemMarkerComponent> _itemMarkerPool;
    private IItemFactoryService _itemFactoryService;

    private bool _receiveItemCreateRequest = true;

    public ItemSetupSystem(IItemFactoryService itemFactoryService)
    {
        _itemFactoryService = itemFactoryService;
    }

    public void Init(IEcsSystems systems)
    {
        var world       = systems.GetWorld();
        _filter         = world.Filter<ItemMarkerComponent>().Inc<InventoryComponent>().End();
        _itemMarkerPool = world.GetPool<ItemMarkerComponent>();

        _itemFactoryService.Load();

    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            if(_receiveItemCreateRequest == true)
            {
                _receiveItemCreateRequest = false;
                ref var itemMarkerComponent = ref _itemMarkerPool.Get(entity);
                if(itemMarkerComponent.marker != null)
                {
                    _itemFactoryService.Create(itemMarkerComponent.marker);
                }
                
            }
            
        }
    }
}
