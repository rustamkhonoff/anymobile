using System.Collections;
using UnityEngine;

public class EntitiesFighting : MonoBehaviour
{
    private EntitiesMediator m_mediator;

    private const float FightDistance = 1.5f;
    private const float CheckoutDelta = 0.1f;
    private const float FigihtDelta = 0.5f;

    public void Setup(EntitiesMediator entitiesMediator)
    {
        m_mediator = entitiesMediator;

        m_mediator.OnEntitiesRecieved += StartCoroutines;
    }

    private void StartCoroutines()
    {
        StartCoroutine(IE_CheckoutEntitiesTarget());
        StartCoroutine(IE_ControlEntitiesFight());
    }

    public void KillAllCoroutines()
    {
        StopAllCoroutines();
    }

    private IEnumerator IE_CheckoutEntitiesTarget()
    {
        var wfs = new WaitForSeconds(CheckoutDelta);

        while (true)
        {
            if (IsEntitiesValid)
            {
                for (int i = 0; i < m_mediator.Entities.Count; i++)
                {
                    Entity item = m_mediator.Entities[i];
                    if (item.Target == null)
                    {
                        var foundTarget = FindClosestEntity(item);

                        if (foundTarget != null)
                        {
                            item.Target = foundTarget;
                            foundTarget.Target = item;

                            foundTarget.CanMove = true;
                            item.CanMove = true;
                        }
                    }
                }
            }

            yield return wfs;
        }
    }

    private IEnumerator IE_ControlEntitiesFight()
    {
        var wfs = new WaitForSeconds(FigihtDelta);
        while (true)
        {
            if (IsEntitiesValid)
            {
                for (int i = 0; i < m_mediator.Entities.Count; i++)
                {
                    Entity item = m_mediator.Entities[i];
                    if (item.Target == null) continue;

                    if (Vector3.Distance(item.transform.position, item.Target.transform.position) < FightDistance)
                    {
                        item.CanMove = false;
                        item.Health -= item.Target.Damage;

                        if (item.Health <= 0)
                            m_mediator.RemoveEntity(item);
                    }
                }
            }
            yield return wfs;
        }
    }

    private bool IsEntitiesValid => m_mediator.Entities != null && m_mediator.Entities.Count != 0;

    public Entity FindClosestEntity(Entity entity)
    {
        float distance = float.MaxValue;
        Vector3 currentPosition = entity.transform.position;
        Entity closestEntity = null;


        foreach (var item in m_mediator.Entities)
        {
            if (item.Target != null || item == entity) continue;

            float currentDist = Vector3.Distance(currentPosition, item.transform.position);

            if (currentDist <= distance)
            {
                distance = currentDist;
                closestEntity = item;
            }
        }

        return closestEntity;
    }
}