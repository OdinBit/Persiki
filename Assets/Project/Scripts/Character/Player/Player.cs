using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private EcsWorld _world;
    private EcsEntity _entity;
    private Rigidbody2D _rb2d;

    [Inject]
    public void Construct(EcsWorld world)
    {
        _world = world;
    }

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (_world == null) return;

        _entity = _world.CreateEntity();
        _world.Add(_entity, new PlayerTag());
        _world.Add(_entity, new Rigidbody2DComponent { Value = _rb2d });
        _world.Add(_entity, new PlayerMovementData { Direction = Vector2.zero, IsMoving = false });
        _world.Add(_entity, new PlayerAttackData { AttackStatus = EAttackStatus.Finished, HasTarget = false });
    }

    private void OnDestroy()
    {
        if (_world != null && _entity.IsValid)
        {
            _world.DestroyEntity(_entity);
        }
    }

    public Vector2 GetPlayerPosition()
    {
        return (Vector2)transform.position;
    }
}
