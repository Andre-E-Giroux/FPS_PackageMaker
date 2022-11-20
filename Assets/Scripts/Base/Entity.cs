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
    public bool ragdollDeathAllowed = false;

    /// <summary>
    /// parent holding the hitNodes
    /// </summary>
    [SerializeField]
    private Transform hitNodesParent;

    /// <summary>
    /// Nodes for explosion/similar effects to use to see if the target can be hit by effect
    /// </summary>
    [HideInInspector]
    public Transform[] hitNodes;

    /// <summary>
    /// Reference tot he RagdolScript component on the entitiy
    /// </summary>
    [SerializeField]
    private RagdollScript ragdollScript;

   // [SerializeField]
    //public Ragdoll ragdoll;

    
    /// <summary>
    /// Add health to target
    /// </summary>
    /// <param name="increase">Float value of the added health to the entity</param>
    public virtual void AddHealth(float increase)
    {
       // Debug.Log("Add health called on entity: " + gameObject.name + " value increase = " + increase);
        if (currentHealth + increase > MAX_HEALTH)
            currentHealth = MAX_HEALTH;
        
        else if(!CheckDeath())
            currentHealth += increase;

        if (ED)
            ED.UpdateHealthBar(currentHealth, MAX_HEALTH);
    }
    

    /// <summary>
    /// Take damage based on the part that was hit
    /// </summary>
    /// <param name="deffaultDamage"></param>
    /// <param name="distance"></param>
    /// <param name="hitBodyPart"></param>
    public void TakeDamageBasedOnPart(float deffaultDamage, float distance, string hitBodyPart)
    {
        // 1. find what type of body part
        // 2. fin algorithm on how to deal damage


        distance = 1;

        Debug.Log("Damaged based on part: " + hitBodyPart);

        if (distance < 1)
            distance = 1;

        if (hitBodyPart == "CritPart")
        {
            Debug.Log("damage: crit");
            currentHealth -= (deffaultDamage / distance) * 2;
        }

        else if (hitBodyPart == "WeakPart")
        {
            Debug.Log("damage: weak");

            currentHealth -= (deffaultDamage / distance) * 1.5f;
        }


        else if(hitBodyPart == "StrongPart")
        {
            Debug.Log("damage: strong");

            currentHealth -= (deffaultDamage / distance) * 0.5f;
        }

        else
        {
            Debug.Log("damage: standard");

            currentHealth -= (deffaultDamage / distance);
        }

        CheckDeath();

        if (ED)
            ED.UpdateHealthBar(currentHealth, MAX_HEALTH);
    }

    private bool CheckDeath()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;

            if (ragdollScript && ragdollDeathAllowed)
                DeathRagdoll();
            else
                DeathDelete();

            return true;
        }

        return false;
    }

    // to be modded later if needed
    public void TakeExploDistanceDamage(float explosioDamage)
    {
        currentHealth -= explosioDamage;

        CheckDeath();

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
    /// Call this function to enable ragdoll
    /// </summary>
    public void DeathRagdoll()
    {
        ragdollScript.SetActiveRagdoll(true);
        ragdollScript.SetExtraObjectConnection(false);
    }

    // Start is called before the first frame update
    void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Entity");
        currentHealth = MAX_HEALTH;

        hitNodes = hitNodesParent.GetComponentsInChildren<Transform>();

        if(ragdollScript)
        {
            ragdollScript.InitializeRagdoll();
            ragdollScript.SetAccessorieOrigins();
        }
    }
}
