using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementService
{
    void MoveToDirection(Rigidbody2D body, Vector2 direction);
}
