using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ItemFactory : IItemFactoryService
{
    private const string SimpleSwordPath = "Prefabs/Weapon/MeleeWeapon/SimpleSword";

    private readonly DiContainer _diContainer;

    private Object _simpleSwordPrefab;
    public ItemFactory(DiContainer diContainer)
    {
        _diContainer = diContainer;
    }
    public void Load()
    {
        _simpleSwordPrefab = Resources.Load(SimpleSwordPath);
    }
    public void Create(ItemMarker marker)
    {
        _diContainer.InstantiatePrefab(_simpleSwordPrefab, 
                                        marker.transform.position, 
                                        Quaternion.identity, 
                                        marker.transform);
    
    }

}
