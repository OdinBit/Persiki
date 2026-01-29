using UnityEngine;

public enum EAttackStatus
{
    Finished,
    InProgress,
    Cancelled
}

public struct PlayerAttackData
{
    public Vector2 TargetPosition;
    public bool HasTarget;
    public EAttackStatus AttackStatus;
}
