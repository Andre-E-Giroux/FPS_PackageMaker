using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] m_particleSystem = null;
    [SerializeField]
    private GameObject m_particleGameObject = null;

    [SerializeField]
    private SphereCollider sCollider;

    [SerializeField]
    private float maxColliderRadiusSize;

    private float minColliderRadiusSize;

    [SerializeField]
    private float collisionSizeIncrease;


    private float maxExplosionDistance;
    private float maxExplosionDamage;
    private float damageToDistanceModifier;

    private void OnEnable()
    {
        sCollider.gameObject.SetActive(true);
        sCollider.radius = minColliderRadiusSize;

    }

    // Update is called once per frame
    void Update()
    {
        if (m_particleGameObject.activeInHierarchy)
        {
            for (int i = 0; i < m_particleSystem.Length; i++)
            {
                if (m_particleSystem[i].IsAlive())
                {
                    return;
                }
            }

            m_particleGameObject.SetActive(false);
        }
    }



    private void FixedUpdate()
    {
        if (sCollider.radius >= maxColliderRadiusSize)
        {
            sCollider.radius = minColliderRadiusSize;
            sCollider.gameObject.SetActive(false);
        }
        if (sCollider.gameObject.activeInHierarchy)
        {
            sCollider.radius = sCollider.radius + collisionSizeIncrease;
        }
    }

    /// <summary>
    /// When an object enters this trigger, verify it has an Entity component, if so deal damage to it
    /// </summary>
    /// <param name="c"></param>
    private void OnTriggerEnter(Collider c)
    {
        Entity cEntity = c.transform.root.GetComponent<Entity>();

        float shortestDistance = float.MaxValue;

        if (cEntity)
        {
            for(int i = 0; i < cEntity.hitNodes.Length; i++)
            {
                RaycastHit hit;
                if (Physics.Raycast(cEntity.hitNodes[i].position, transform.TransformDirection(cEntity.hitNodes[i].position - transform.position), out hit, Mathf.Infinity))
                {
                    Debug.DrawRay(cEntity.hitNodes[i].position, transform.TransformDirection(cEntity.hitNodes[i].position - transform.position) * hit.distance, Color.yellow);

                    if (shortestDistance > hit.distance)
                        shortestDistance = hit.distance;
                }
            }
            cEntity.TakeExploDistanceDamage(-CalculateExplosionDamage(shortestDistance));

        }

    }

    /// <summary>
    /// Set explosion damage from the projectile object to the explosion object
    /// </summary>
    /// <param name="maxExplosionDistance">projectile's maxExplosionDistance value</param>
    /// <param name="maxExplosionDamage">projectile's maxExplosionDamage value</param>
    /// <param name="damageToDistanceModifier">projectile's damageToDistanceModifier value</param>
    public void SetDamage(float maxExplosionDistance, float maxExplosionDamage, float damageToDistanceModifier)
    {
        this.maxExplosionDistance =  maxExplosionDistance;
        this.maxExplosionDamage = maxExplosionDamage;
        this.damageToDistanceModifier = damageToDistanceModifier;
    }

    /// <summary>
    /// Caculate explosion damage of an entity that was hit by the explosion, the calculation takes into account distance between objects and the values set by the SetDamage function
    /// </summary>
    /// <param name="currentDistanceToTarget">distance between explosion point and target hit</param>
    /// <returns>returns damage value to be removed from the entity</returns>
    private float CalculateExplosionDamage(float currentDistanceToTarget)
    {
        float damage = maxExplosionDamage - ((maxExplosionDistance - currentDistanceToTarget) * damageToDistanceModifier);

        return Mathf.Clamp(damage, 0, maxExplosionDamage); ;
    }

}
