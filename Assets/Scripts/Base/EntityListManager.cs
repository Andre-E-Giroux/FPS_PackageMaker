using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityListManager : ManagerAssistant
{
    public List<Entity> entities = new List<Entity>();

    public override void StartManager()
    {
       /* entities = new List<Entity>();

        foreach (Entity entity in FindObjectsOfType<Entity>())
            AddTransformToActors(entity);*/
    }

    public void Start()
    {
        entities = new List<Entity>();

        foreach (Entity entity in FindObjectsOfType<Entity>())
            AddTransformToActors(entity);


        GameManager gm = FindObjectOfType<GameManager>();
        if (gm)
            headManager = gm.AddSceneManagerAssistants(this);
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
