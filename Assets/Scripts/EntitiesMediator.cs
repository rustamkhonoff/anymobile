using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntitiesMediator : MonoBehaviour
{
    public event Action OnEntitiesRecieved;

    [SerializeField] private EntityCreator _creator;
    [SerializeField] private EntitiesFighting _fighting;

    public List<Entity> Entities { get; set; }

    private void Awake()
    {
        _fighting.Setup(this);

        _creator.EntitiesCreated += HandleEntitiesCreated;
    }

    private void HandleEntitiesCreated(IEnumerable<Entity> entities)
    {
        Entities = entities.ToList();

        OnEntitiesRecieved?.Invoke();
    }

    public void RemoveEntity(Entity entity)
    {
        if (!Entities.Contains(entity)) return;

        Entities.Remove(entity);
        Destroy(entity.gameObject);

        if (Entities.Count == 1)
            End();
    }
    private void End()
    {
        _fighting.KillAllCoroutines();

        Debug.Log(Entities.First() + " WINS!");
    }
}
