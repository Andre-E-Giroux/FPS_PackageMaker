using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField]
    private EntityDebugger ED;


    /// <summary>
    /// Entity's current health
    /// </summary>
    public float currentHealth = 100;

    /// <summary>
    /// Entity's max health
    /// </summary>
    public const float MAX_HEALTH = 100;

    /// <summary>
    /// Is this entity the player?
    /// </summary>
    public bool isPlayer = false;

    /// <summary>
    /// Can the entity ragdoll, true of false?
    /// If true the DeathRagdoll function will be called, else DeathDelete function will be called
    /// </summary>
    public bool ragdoll = false;
   
    /// <summary>
    /// Add health to target
    /// </summary>
    /// <param name="increase">Float value of the added health to the entity</param>
    public void AddHealth(float increase)
    {
        Debug.Log("Add health called on entity: " + gameObject.name + " value increase = " + increase);
        if (currentHealth + increase > MAX_HEALTH)
            currentHealth = MAX_HEALTH;
        else if ((currentHealth + increase) <= 0)
        {
            currentHealth = 0;
            if (ragdoll)
                DeathRagdoll();
            else
                DeathDelete();
        }
        else
            currentHealth += increase;

        if (ED)
            ED.UpdateHealthBar(currentHealth, MAX_HEALTH);
    }

    /// <summary>
    /// Delete game object
    /// </summary>
    public void DeathDelete()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    public void DeathRagdoll()
    {

    }

    // Start is called before the first frame update
    void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Entity");
        currentHealth = MAX_HEALTH;
    }
}
