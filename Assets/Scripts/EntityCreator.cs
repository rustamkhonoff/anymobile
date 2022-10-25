using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
public class EntityCreator : MonoBehaviour
{
    public event Action<IEnumerable<Entity>> EntitiesCreated;

    [SerializeField] private Entity _entityPrefab;
    [Space]
    [SerializeField] private int _createCount;
    [SerializeField] private float _spawnRadius;
    [Header("Characteristic")]
    [SerializeField] private Vector2 _healthRange;
    [SerializeField] private Vector2 _damageRange;

    private List<Entity> m_createdEntities;

    private void Start()
    {
        CreateAll();
    }

    private void CreateAll()
    {
        m_createdEntities = new List<Entity>();

        for (int i = 0; i < _createCount; i++)
        {
            var tempCreated = CreateAt(RandomSpawnPosition);
            m_createdEntities.Add(tempCreated);
        }

        EntitiesCreated?.Invoke(m_createdEntities);
    }

    private Vector3 RandomSpawnPosition
    {
        get
        {
            var temp = Random.insideUnitSphere;
            temp.y = 0;

            return temp * _spawnRadius;
        }
    }

    public Entity CreateAt(Vector3 vector3)
    {
        var temp = Instantiate(_entityPrefab, vector3, Quaternion.identity);
        temp.SetCharacteristic(_healthRange.GetRandom(), _damageRange.GetRandom());

        return temp;
    }
}
