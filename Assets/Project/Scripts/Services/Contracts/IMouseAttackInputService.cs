using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMouseAttackInputService
{
    System.Action<Vector3> OnAttackPressed { get; set; }
}
