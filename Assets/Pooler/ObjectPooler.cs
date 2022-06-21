using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
    
    public string poolerID = "";

    [SerializeField]
    public GameObject pooledObject;
    [SerializeField]
    protected int poolAmount = 10;
    [SerializeField]
    protected bool willGrow = true;
    /// If the maximum amount of bjects have been reach, would this script reuse an active object
    [SerializeField]
    protected bool willDisableWhenNeeded = false;
    [SerializeField]
    protected List<GameObject> pooledObjects = new List<GameObject>();
    [SerializeField]
    protected List<GameObject> activeGameObjects = new List<GameObject>();

    // Use this for initialization
    void Awake() {
        
        // instantiate game objects equal to the poolAmount variable and pool them.
        for (int i = 0; i < poolAmount; i++)
        {
            GameObject obj = Instantiate(pooledObject);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    /// <summary>
    /// Activate pooled objects
    /// </summary>
    /// <returns>Available object</returns>
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            // if an object is not set to active set to active
            if (!pooledObjects[i].activeInHierarchy)
            {
                activeGameObjects.Add(pooledObjects[i]);
                return pooledObjects[i];
            }
        }
        // if there are no available objects in pool, Instantiate more if allowed
        if (willGrow)
        {
            GameObject obj = Instantiate(pooledObject);
            pooledObjects.Add(obj);
            obj.SetActive(false);
            return obj;
        }
        // if there are no available objects in pool and cannot instantiate more, reuse one
        else if (willDisableWhenNeeded)
        {
            GameObject reUsedObject = activeGameObjects[0];
            reUsedObject.SetActive(false);
            RemoveObjectOfActiveList(activeGameObjects[0]);
            activeGameObjects.Add(reUsedObject);
            return reUsedObject;
        }

        return null;

    }



   /// <summary>
   /// Remove object from active list
   /// </summary>
   /// <param name="obj">Object to be removed</param>
    public void RemoveObjectOfActiveList(GameObject obj)
    {
        activeGameObjects.Remove(obj);
        obj.SetActive(false);
        obj.transform.parent = null;
    }

    /// <summary>
    /// Gets the objects that are active in this pool
    /// </summary>
    /// <returns>Returns the active object list</returns>
    public List<GameObject> GetActiveObjectList()
    {
        return activeGameObjects;
    }

    /// <summary>
    /// Get the number of pooled scripts
    /// </summary>
    /// <returns>Pool amount</returns>
    public int GetPooledAmount()
    {
        return poolAmount;
    }
  

    
 
    /// <summary>
    /// Clean all lists and reset pool
    /// </summary>
    public void CleanPool()
    {
        activeGameObjects.Clear();

        for(int i = 0; i < pooledObjects.Count; i++)
        {
            pooledObjects[i].SetActive(false);
        }
    }
   


}
