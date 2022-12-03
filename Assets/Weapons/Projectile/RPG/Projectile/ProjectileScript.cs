using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    /// <summary>
    /// Projectile rigidbody
    /// </summary>
    protected Rigidbody rb;

    // Entity target LayerMask, layer mask of those that will trigger unique effect
    [SerializeField]
    protected LayerMask entityTargetLayerMask;

    /// <summary>
    /// Speed of projectile
    /// </summary>
    [SerializeField]
    protected float projectileSpeedForce = 1;

    /// <summary>
    /// Damage done to entity on direct target
    /// </summary>
    [SerializeField]
    protected float directDamage = 60;

    // Explosion physics force
    public float weaponInanimateImpactForce = 0f;


    public void Start()
    {
        if (!rb)
            rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        //reset and set projectile velocity

        if(!rb)
            rb = GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;
        rb.AddForce(transform.forward * projectileSpeedForce, ForceMode.Impulse);
    }

    private void WeaponAddPhysicsForce(Rigidbody rgHit, Transform hitTransform)
    {
        if (!rgHit)
            Debug.LogError("Projectile trying to add force to a non rigidbody");

        float distance = (hitTransform.position - transform.position).magnitude;

        rgHit.AddForceAtPosition(((hitTransform.position - transform.position).normalized) * (weaponInanimateImpactForce / distance), hitTransform.position, ForceMode.Impulse);
    }



    protected virtual void OnTriggerEnter(Collider other)
    {
        Rigidbody rgHit = other.transform.GetComponent<Rigidbody>();

        Entity entityHit = other.transform.root.GetComponent<Entity>();

        // if target hit is included in layer Mask
        if (entityHit)
        {
            Debug.Log("Damage target!");
            //other.GetComponent<Entity>().AddHealth(-directDamage);

            if (entityHit.isAlive)
            {
                Debug.Log("is alive! proj");
                if (entityHit.TakeDamageBasedOnPart(directDamage, 0, other.transform.tag))
                {
                    Debug.Log("just died proj");

                    WeaponAddPhysicsForce(other.GetComponent<Rigidbody>(), other.transform);
                }
            }

            if(!entityHit.isAlive)
            {
                WeaponAddPhysicsForce(other.GetComponent<Rigidbody>(), other.transform);
            }

        }
        else if(rgHit)
        {
            WeaponAddPhysicsForce(other.GetComponent<Rigidbody>(), other.transform);
        }

        gameObject.SetActive(false);


    }


}
