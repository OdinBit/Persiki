using System;
using UnityEngine;

public enum ECharacterTypes
{
    PLAYER,
    ENEMY
}
public class CharacterContext
{

    private Weapon _currentWeapon;
    private GameObject _character;
    private ECharacterTypes _type;

    public CharacterContext(Weapon weapon, GameObject character, ECharacterTypes type = ECharacterTypes.ENEMY)
    {
        _currentWeapon = weapon;
        _character = character;
        _type = type;
    }

    public ECharacterTypes GetTypes() { return _type; }

    // === Weapon ===
    public Weapon GetCurrentWeapon() => _currentWeapon;
    public void SetCurrentWeapon(Weapon weapon) => _currentWeapon = weapon;

    // === Target ===
    public GameObject GetCharacter() => _character;
    public void SetCharacter(GameObject target) => _character = target;
}
