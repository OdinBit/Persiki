using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IMovable, IService
{
    private PlayerMoveFsm       _playerFsm;
    private PlayerAttackFsm     _playerAttackFsm;
    private IMovementService    _movementService;
    private EventBus            _eventBus;
    private Rigidbody2D         _rb2d;    

    [Inject]
    public void Construct(EventBus eventBus, IMovementService movementService)
    {
        _eventBus = eventBus;
        _movementService = movementService;
        _rb2d = GetComponent<Rigidbody2D>();
        _playerFsm = new PlayerMoveFsm(_movementService, _rb2d);
        _playerAttackFsm = new PlayerAttackFsm(_eventBus);

        _eventBus.Subscribe<MouseAttackInputSignal>(AttackEventHandler);
        Debug.Log("Player initialized");
    }

    private void Update()
    {
        _playerAttackFsm.FsmRun();
    }

    private void FixedUpdate()
    {
        _playerFsm.FsmRun();
    }

    public void MoveEvent(Vector2 direction)
    {
        _playerFsm.PlayerDirectionUpdate(direction);
    }
    public void AttackEventHandler(MouseAttackInputSignal signal)
    {
        _playerAttackFsm.TargetPositionUpdate(signal.worldPos);
    }

    public Vector2 GetPlayerPosition()
    {
        return (Vector2)gameObject.transform.position;
    }
}
