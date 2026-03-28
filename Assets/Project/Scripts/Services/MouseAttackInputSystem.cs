using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
public class MouseAttackInputSystem : IMouseAttackInputService
{
    private GameInput _gameInput;

    public System.Action<Vector3> OnAttackPressed { get; set; }

    public MouseAttackInputSystem(GameInput gi)
    {
        _gameInput = gi;
        _gameInput.Gameplay.Attack.performed += OnAttackInput;
        _gameInput.Enable();
    }

    private void OnAttackInput(InputAction.CallbackContext ctx)
    {
        Vector3 worldPos = ReadMouseWorldPosition();
        OnAttackPressed?.Invoke(worldPos);
    }

    private Vector3 ReadMouseWorldPosition()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        return Camera.main.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane)
        );
    }
}