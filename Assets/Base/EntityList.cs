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

    
    

}
