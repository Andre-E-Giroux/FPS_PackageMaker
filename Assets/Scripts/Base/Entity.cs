using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all NPC/Player/Object with health/etc
/// </summary>
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

    private PlayerSM playerSM;

    /// <summary>
    /// Can the entity ragdoll, true of false?
    /// If true the DeathRagdoll function will be called, else DeathDelete function will be called
    /// </summary>
    public bool ragdollDeathAllowed = false;

    /// <summary>
    /// Boolean that informs if the entity is dead or not
    /// </summary>
    public bool isAlive;

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
    ///  Take damage based on the part that was hit
    /// </summary>
    /// <param name="deffaultDamage"></param>
    /// <param name="distance"></param>
    /// <param name="hitBodyPart"></param>
    /// <returns>Returns True if the entity is dead, false if entiti is still alive</returns>
    public bool TakeDamageBasedOnPart(float deffaultDamage, float distance, string hitBodyPart)
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

        

        if (ED)
            ED.UpdateHealthBar(currentHealth, MAX_HEALTH);

        return(CheckDeath());
    }

    /// <summary>
    /// Verify if the entity has died
    /// </summary>
    /// <returns>True: the enity has died, false: the entity is still alive</returns>
    private bool CheckDeath()
    {
        if (!isAlive)
            return true;

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            if (ragdollScript && ragdollDeathAllowed)
                ragdollScript.ActivateRagdoll(true);
            else
            {
                if(isPlayer)
                {
                    playerSM.PlayerDeath(true);
                }
               
            }

            isAlive = false;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Take explosion damage when hit
    /// </summary>
    /// <param name="explosionDamage">damage done by the explosion</param>
    public void TakeExploDistanceDamage(float explosionDamage)
    {
        currentHealth -= explosionDamage;

        CheckDeath();

        if (ED)
            ED.UpdateHealthBar(currentHealth, MAX_HEALTH);
    }


    /// <summary>
    /// Delete entity
    /// </summary>
    public void DeathDelete()
    {
        Destroy(gameObject);
    }

   
    void Awake()
    {
        isAlive = true;
        if(isPlayer)
            playerSM = GetComponent<PlayerSM>();
        gameObject.layer = LayerMask.NameToLayer("Entity");
        currentHealth = MAX_HEALTH;

        hitNodes = hitNodesParent.GetComponentsInChildren<Transform>();

        if(ragdollScript)
        {
            ragdollScript.InitializeRagdoll();
        }
    }
}
