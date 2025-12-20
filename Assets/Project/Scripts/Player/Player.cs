using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IDamageable, IMovable, IService
{
    private PlayerMoveFsm       _playerFsm;
    private PlayerAttackFsm     _playerAttackFsm;
    private IMovementService    _movementService;
    private EventBus            _eventBus;

    [Inject]
    public void Construct(EventBus eventBus, IMovementService movementService)
    {
        _eventBus = eventBus;
        Debug.Log("Player Init");
        _movementService = movementService;

        _playerFsm = new PlayerMoveFsm(_movementService.MoveToDirection, GetComponent<Rigidbody2D>());
        _playerAttackFsm = new PlayerAttackFsm(_eventBus);

        _eventBus.Subscribe<MouseAttackInputSignal>(AttackEventHandler);
    }

    private void Update()
    {
        _playerFsm.FsmRun();
        _playerAttackFsm.FsmRun();
    }

    public void MoveEvent(Vector2 direction)
    {
        _playerFsm.PlayerDirectionUpdate(direction);
    }
    public void AttackEventHandler(MouseAttackInputSignal signal)
    {
        _playerAttackFsm.TargetPositionUpdate(signal.worldPos);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Player take damage");
    }

    public Vector2 GetPlayerPosition()
    {
        return (Vector2)gameObject.transform.position;
    }
}
