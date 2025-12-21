using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;
using Zenject;

public class MovementInputHandler : IKeyboardMoveInputService, ITickable, System.IDisposable
{
    private GameInput   _gameInput;
    private Vector2     _moveDirection;

    public MovementInputHandler(GameInput gameInput)
    {
        _moveDirection = Vector2.zero;
        _gameInput = gameInput;
        _gameInput.Enable();
       
    }
    public void Dispose() => _gameInput.Dispose();


    public void Tick()
    {
        ReadMoveDirection();
    }

    private void ReadMoveDirection()
    {
        var direction = _gameInput.Gameplay.Movement.ReadValue<Vector2>();
        _moveDirection = direction;
    }

    public Vector2 GetMoveDirection()
    {
        return _moveDirection;
    }
}
