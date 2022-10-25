using System.Linq;
using UnityEngine;

public class EntityCamera : MonoBehaviour
{
    [SerializeField] private EntitiesMediator _mediator;
    [SerializeField] private float _speed;

    private Vector3 _offset;
    private Entity _target;
    private Entity _targetEnemy;

    private void Awake()
    {
        _offset = transform.position;
        _mediator.OnEntitiesRecieved += Setup;
    }

    private void Setup()
    {
        SetTarget(_mediator.Entities.First());
    }

    private void SetTarget(Entity entity)
    {
        _target = entity;
        _targetEnemy = _target.Target;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.TryGetComponent(out Entity entity))
                {
                    SetTarget(entity);
                }
            }
        }

        if (_targetEnemy != null)
            SetTarget(_targetEnemy);
        else
            SetTarget(_mediator.Entities.First());

        if (_target != null)
            transform.position = Vector3.Lerp(transform.position, _target.transform.position + _offset, Time.deltaTime * _speed);
    }
}
