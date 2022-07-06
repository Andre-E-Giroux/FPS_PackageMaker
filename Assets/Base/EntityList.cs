using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityList : MonoBehaviour
{
    public List<Entity> entities = new List<Entity>();

    private void Start()
    {
        foreach (Entity entity in FindObjectsOfType<Entity>())
            AddTransformToActors(entity);
    }

    public void AddTransformToActors(Entity entity)
    {
        entities.Add(entity);
    }
    public void RemoveTransformFromActors(Entity entity)
    {
        entities.Remove(entity);
    }

    // explosion
    public void CheckForExplosionDistance(Transform explosionPoint, float maxDistance, float explosionDamage)
    {
        
        foreach (Entity entity in entities)
        {
            Debug.Log("Explosion each");
            Debug.Log("distance: " + (explosionPoint.position - entity.transform.position).magnitude);
            if((explosionPoint.position - entity.transform.position).magnitude < maxDistance)
            {
                //within max distance
                Debug.Log("Damaga them! Entity: " + entity.gameObject.name);
                

                entity.AddHealth(-(entity.transform.position - explosionPoint.position).magnitude * explosionDamage);
               
                /*if(tempHandler.health <= 0)
                {
                    RagdollActivate tempRagActive = actor.GetComponent<RagdollActivate>();
                    foreach (Rigidbody rbs in tempRagActive._rigidbodies)
                    {
                        Debug.Log("alert");
                        rbs.AddExplosionForce(pushForce, transform.position, 100);
                    }
                }*/
            }
        }
    }
    

}
