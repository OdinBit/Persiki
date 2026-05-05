using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemFactoryService
{
    public void Load();
    public void Create(ItemMarker marker);
}
