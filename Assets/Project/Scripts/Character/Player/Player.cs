using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private PlayerMoveFsm               _playerFsm;
    private PlayerAttackFsm             _playerAttackFsm;
    private IMovementService            _movementService;
    private IKeyboardMoveInputService   _keyboardMoveInputService;
    private EventBus                    _eventBus;
    private Rigidbody2D                 _rb2d;    

    [Inject]
    public void Construct(  EventBus                    eventBus, 
                            IMovementService            movementService,
                            IKeyboardMoveInputService   keyboardMoveInputService)
    {
        _eventBus = eventBus;
        _movementService = movementService;
        _keyboardMoveInputService = keyboardMoveInputService;
        _rb2d = GetComponent<Rigidbody2D>();
        _playerFsm = new PlayerMoveFsm(_keyboardMoveInputService, _movementService, _rb2d);
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
    public void AttackEventHandler(MouseAttackInputSignal signal)
    {
        _playerAttackFsm.TargetPositionUpdate(signal.worldPos);
    }
    public Vector2 GetPlayerPosition()
    {
        return (Vector2)gameObject.transform.position;
    }
}
