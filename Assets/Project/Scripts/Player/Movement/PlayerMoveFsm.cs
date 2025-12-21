using System;
using UnityEngine;
using UnityHFSM;

public class PlayerMoveFsm
{
    private StateMachine              _fsm;
    private PlayerMovementData        _playerMovementFsmData;
    private readonly IMovementService _movementService;
    private readonly Rigidbody2D _player_rb;

    public PlayerMoveFsm(IMovementService movementService, Rigidbody2D player_rb)
    {
        _movementService = movementService;
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
        _fsm.OnLogic();
    }

    public void PlayerDirectionUpdate( Vector2 dir )
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

//void Start()
//{


//    fsm.SetStartState("Idle");
//    fsm.AddState("Idle", onLogic: state => { Debug.Log("Idle state"); });
//    fsm.AddState("Walk", onLogic: state => { Debug.Log("Walk state"); });
//    fsm.AddState("Attack", onLogic: state => { Debug.Log("Attack state"); });
//    fsm.AddState("GetDamage", onLogic: state => { Debug.Log("GetDamage state"); });
//    fsm.AddState("Death", onLogic: state => { Debug.Log("Death state"); });


//    fsm.AddTransition("Idle", "Walk",
//        transition => Input.GetKeyDown(KeyCode.W));
//    fsm.AddTransition("Walk", "Idle",
//        transition => Input.GetKeyUp(KeyCode.W));

//    fsm.AddTransition("Idle", "Attack",
//        transition => Input.GetMouseButtonDown(0));
//    fsm.AddTransition("Attack", "Idle",
//        transition => !Input.anyKey);

//    fsm.AddTransition("Idle", "GetDamage",
//        transition => ShoodTakeDamage());
//    fsm.AddTransition("GetDamage", "Death",
//        transition => Die());
//    fsm.AddTransition("GetDamage", "Idle",
//        transition => !ShoodTakeDamage());

//    fsm.AddTransition("Walk", "GetDamage",
//        transition => ShoodTakeDamage());
//    fsm.AddTransition("GetDamage","Walk", 
//        transition => Input.GetKeyDown(KeyCode.W));

//    fsm.AddTransition("Walk", "Attack",
//        transition => Input.GetMouseButtonDown(0));
//    fsm.AddTransition("Attack","Walk",
//        transition => Input.GetKey(KeyCode.W)); 

//    fsm.AddTransition("Attack", "GetDamage",
//        transition => ShoodTakeDamage());
//    fsm.AddTransition("GetDamage","Attack", 
//        transition => !ShoodTakeDamage() && Input.GetMouseButton(0));

//    fsm.Init();
//}
//void Update()
//{
//    fsm.OnLogic();
//}

//bool ShoodTakeDamage()
//{
//    if (Input.GetKeyDown(KeyCode.D))
//    {
//        Debug.Log("Player took damage");
//        HP -= 50;
//        return true;
//    }
//    else return false;
//}

//bool Die()
//{
//    if (HP <= 0)
//    {
//        Debug.Log("Player died");
//        return true;
//    }
//    else return false;
//}