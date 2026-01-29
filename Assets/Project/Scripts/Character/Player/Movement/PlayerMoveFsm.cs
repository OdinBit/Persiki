using System;
using UnityEngine;
using UnityHFSM;

public class PlayerMoveFsm
{
    private StateMachine                        _fsm;
    private PlayerMovementData                  _playerMovementFsmData;
    private readonly IMovementService           _movementService;
    private readonly IKeyboardMoveInputService  _keyboardMoveInputService;
    private readonly Rigidbody2D                _player_rb;

    public PlayerMoveFsm(   IKeyboardMoveInputService keyboardMoveInputService, 
                            IMovementService movementService, 
                            Rigidbody2D player_rb)
    {
        _movementService = movementService;
        _keyboardMoveInputService = keyboardMoveInputService;
        _player_rb = player_rb;

        _fsm = new StateMachine();

        _fsm.SetStartState("Idle");

        _fsm.AddState("Idle", 
            onEnter: state => FsmIdleStateEnter(), 
            onLogic: state => FsmIdleState(),
            onExit:  state => FsmIdleStateExit());

        _fsm.AddState("Move",
            onEnter: state => FsmMoveStateEnter(),
            onLogic: state => FsmMoveState(),
            onExit:  state => FsmMoveStateExit());

        _fsm.AddTransition("Idle", "Move", FsmTransitionGuardIdleToMove);
        _fsm.AddTransition("Move", "Idle", FsmTransitionGuardMoveToIdle);

        _fsm.Init();
    }

    public void FsmRun()
    {
        PlayerMoveDirectionUpdate(_keyboardMoveInputService.GetMoveDirection());
        _fsm.OnLogic();
    }

    public void PlayerMoveDirectionUpdate( Vector2 dir )
    {
        _playerMovementFsmData.Direction = dir;
    }

    private void FsmIdleStateEnter()
    {
        //Debug.Log("Idle onEnter");
    }

    private void FsmIdleState()
    {
        //Debug.Log("Idle onLogic");
        _movementService.MoveToDirection(_player_rb, Vector2.zero);
    }

    private void FsmIdleStateExit()
    {
        //Debug.Log("Idle onExit");
    }

    private void FsmMoveStateEnter()
    {
        //Debug.Log("Move onEnter");
    }

    private void FsmMoveState()
    {
        //Debug.Log("Move onLogic");
        _movementService.MoveToDirection(_player_rb, _playerMovementFsmData.Direction);
    }

    private void FsmMoveStateExit()
    {
        //Debug.Log("Move onExit");
    }

    private bool FsmTransitionGuardIdleToMove(Transition<string> transition)
    {
        bool isTransitionAllowed = false;

        if (_playerMovementFsmData.Direction.sqrMagnitude > 0.01f) isTransitionAllowed = true;

        return isTransitionAllowed;
    }

    private bool FsmTransitionGuardMoveToIdle(Transition<string> transition)
    {
        bool isTransitionAllowed = false;

        if (_playerMovementFsmData.Direction.sqrMagnitude < 0.01f) isTransitionAllowed = true;

        return isTransitionAllowed;
    }
}