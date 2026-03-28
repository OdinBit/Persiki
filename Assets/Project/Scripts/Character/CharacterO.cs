using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterO : MonoBehaviour,
                         IHealthHolder,
                         IDamageable
{
    private CharacterContext _context;
    private ICombatService _combatService;
    [SerializeField] private Weapon _weapon; // temp
    public int HealthPoints { get; set; }

    [Inject]
    public void Construct(ICombatService combatService)
    {
        _context = new CharacterContext(_weapon, gameObject, ECharacterTypes.PLAYER); // temp
        _combatService = combatService;
        _combatService.SetContext(_context); // temp
    }

    public void OnDamage()
    {
        
    }

    void Start()
    {
        
    }
    void Update()
    {
        _combatService.Update();
    }
}
