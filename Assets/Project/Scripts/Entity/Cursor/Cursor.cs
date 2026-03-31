using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Leopotam.EcsLite;

public class Cursor : MonoBehaviour
{
    private EcsWorld _world;
    private int _entity;
    [SerializeField] private Vector3 _offset;

    [Inject]
    public void Construct(EcsWorld world)
    {
        _world = world;
    }

    private void Awake()
    {

    }

    private void Start()
    {
        if (_world == null) return;

        _entity = _world.NewEntity();

        ref var pos         = ref _world.GetPool<CursorPositionComponent>().Add(_entity);
        pos.WorldPosition   = transform.position;

        ref var view        = ref _world.GetPool<CursorViewComponent>().Add(_entity);
        view.Transform      = transform;
        view.Offset         = _offset;
    }

    private void OnDestroy()
    {
        if (_world != null && _world.IsAlive() && _entity >= 0)
        {
            _world.DelEntity(_entity);
        }
    }

}
