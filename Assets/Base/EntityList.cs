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
    public void CheckForExplosionDistance(Vector3 explosionPoint, float maxDistance, float explosionDamage)
    {
        
        foreach (Entity entity in entities)
        {
            Debug.Log("Explosion each");
            Debug.Log("distance: " + (explosionPoint - entity.transform.position).magnitude);

           

            if ((explosionPoint - entity.transform.position).magnitude < maxDistance)
            {
                //within max distance

                RaycastHit hit;


                //
                // Does the ray intersect any objects excluding the player layer
                Debug.DrawRay(explosionPoint, transform.TransformDirection((entity.transform.position - explosionPoint)), Color.magenta, 60f);
                Debug.Log("Explosion point = " + explosionPoint);





              
                if (Physics.Raycast(explosionPoint, transform.TransformDirection((entity.transform.position - explosionPoint)), out hit) && hit.transform != null)
                {
                    Debug.Log("Welp!");
                    Debug.DrawRay(transform.position, transform.TransformDirection((entity.transform.position - explosionPoint)).normalized * hit.distance, Color.yellow);
                    Debug.Log("Did Hit");

                    Debug.Log("Damaga them! Entity: " + entity.gameObject.name);


                    entity.AddHealth(-(entity.transform.position - explosionPoint).magnitude * explosionDamage);
                }


                Debug.Log("Alert!");
                
               
            }
        }
    }
    

}
