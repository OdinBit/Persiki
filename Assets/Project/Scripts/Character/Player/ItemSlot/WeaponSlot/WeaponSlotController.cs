using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotController : MonoBehaviour, IService
{
    private Transform _slotTransform;

    public Transform GetWeaponSlotTransform()
    {
        return transform;
    }
}
