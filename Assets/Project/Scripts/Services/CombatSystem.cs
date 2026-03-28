using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : ICombatService
{
    private CombatStateMachine          _stateMachine;
    private CharacterContext            _context;
    private IMouseAttackInputService    _mouseAttackInputService;

    public CombatSystem(IMouseAttackInputService mouseAttackInputService)
    {
        _mouseAttackInputService = mouseAttackInputService;
        
    }

    public void SetContext(CharacterContext ctx) // temp
    {
        _context = ctx;
        _stateMachine = new CombatStateMachine(_context);
        if (_context.GetTypes() == ECharacterTypes.PLAYER) _mouseAttackInputService.OnAttackPressed += OnAttackPressed;
    }

    public void Update()
    {
        _stateMachine.FsmRun();
    }

    private void OnAttackPressed(Vector3 attackPosition)
    {
        _stateMachine.TargetPositionUpdate(attackPosition);
    }
}
