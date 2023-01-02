using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that keeps track of all of the entities in the scene
/// </summary>
public class EntityListManager : ManagerAssistant
{
    public List<Entity> entities = new List<Entity>();


    // Currently unused, stated to replace start.
    public override void StartManager(){}

    public void Start()
    {
        entities = new List<Entity>();

        foreach (Entity entity in FindObjectsOfType<Entity>())
            AddEntityFromList(entity);


        GameManager gm = FindObjectOfType<GameManager>();
        if (gm)
            headManager = gm.AddSceneManagerAssistants(this);
    }

    /// <summary>
    /// Add entity to list
    /// </summary>
    /// <param name="entity">Enity to be added</param>
    public void AddEntityFromList(Entity entity)
    {
        entities.Add(entity);
    }
    /// <summary>
    /// Remove entity from list
    /// </summary>
    /// <param name="entity">Enity to be removed</param>
    public void RemoveEntityFromList(Entity entity)
    {
        entities.Remove(entity);
    }

    
    

}
