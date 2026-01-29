//#define DEBUG_MODE
using CustomEventBus;
using CustomEventBus.Signals;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

public class PlayerAttackFsm
{
    private readonly StateMachine _fsm;
    private EventBus _eventBus;
    private PlayerAttackData _playerAttackFsmData;

    public PlayerAttackFsm(EventBus eventBus)
    {
        _eventBus = eventBus;
        _fsm = new StateMachine();

        _fsm.SetStartState("Passive");

        _fsm.AddState("Passive",
            onEnter: state => FsmPassiveStateEnter(),
            onLogic: state => FsmPassiveState(),
            onExit:  state => FsmPassiveStateExit());

        _fsm.AddState("Attack",
            onEnter: state => FsmAttackStateEnter(),
            onLogic: state => FsmAttackState(),
            onExit:  state => FsmAttackStateExit());

        _fsm.AddTransition("Passive", "Attack", FsmTransitionGuardPassiveToAttack);
        _fsm.AddTransition("Attack", "Passive", FsmTransitionGuardAttackToPassive);

        _fsm.Init();

        _eventBus.Subscribe<PlayerAttackResponseSignal>(AttackFinished);
    }

    public void FsmRun()
    {
        _fsm.OnLogic();
    }

    public void AttackFinished(PlayerAttackResponseSignal signal)
    {
        _playerAttackFsmData.AttackStatus = EAttackStatus.Finished;
    }
    public void TargetPositionUpdate(Vector2 newTargetPosition)
    {
        _playerAttackFsmData.TargetPosition = newTargetPosition;
        _playerAttackFsmData.HasTarget = true;
    }

    private void FsmPassiveStateEnter()
    {
#if DEBUG_MODE
        Debug.Log("Passive onEnter");
#endif
    }
    private void FsmPassiveState()
    {
#if DEBUG_MODE
        Debug.Log("Passive onLogic");
#endif
    }
    private void FsmPassiveStateExit()
    {
#if DEBUG_MODE
        Debug.Log("Passive onExit");
#endif
    }
    private void FsmAttackStateEnter()
    {
#if DEBUG_MODE
        Debug.Log("Attack onEnter");
#endif
        _playerAttackFsmData.AttackStatus = EAttackStatus.InProgress;
        _eventBus.Invoke(new PlayerAttackRequestSignal(_playerAttackFsmData.TargetPosition));
    }
    private void FsmAttackState()
    {
#if DEBUG_MODE
        Debug.Log("Attack onLogic");
#endif
    }
    private void FsmAttackStateExit()
    {
        _playerAttackFsmData.HasTarget = false;
#if DEBUG_MODE
        Debug.Log("Attack onExit");
#endif
    }

    private bool FsmTransitionGuardPassiveToAttack(Transition<string> transition)
    {
        bool isTransitionAllowed = false;
        if(_playerAttackFsmData.HasTarget) isTransitionAllowed = true;
        return isTransitionAllowed;
    }

    private bool FsmTransitionGuardAttackToPassive(Transition<string> transition)
    {
        bool isTransitionAllowed = false;
        if (_playerAttackFsmData.AttackStatus == EAttackStatus.Finished) isTransitionAllowed = true;
        return isTransitionAllowed;
    }


}
