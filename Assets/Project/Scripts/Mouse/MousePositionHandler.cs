using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class MousePositionHandler : ITickable
{
    private Vector3 _worldPos;

    public Vector2 GetCursorePosition()
    {
        return _worldPos;
    }
    private void ReadMousePosition()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        _worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane));
    }

    public void Tick()
    {
        ReadMousePosition();
    }
}
