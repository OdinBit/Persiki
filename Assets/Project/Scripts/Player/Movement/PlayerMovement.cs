using UnityEngine;

public class PlayerMovement : IMovementService
{
    [SerializeField] private float _speed = 5f;

    public PlayerMovement(float speed)
    {
        _speed = speed;
        Debug.Log("PlayerMovement initialized");
    }
    public void MoveToDirection(Rigidbody2D rigidbody, Vector2 direction)
    {
        if (rigidbody != null && direction != null) rigidbody.velocity = direction * _speed;
    }
}
