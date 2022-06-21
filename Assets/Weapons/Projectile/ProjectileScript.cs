using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    /// <summary>
    /// Projectile rigidbody
    /// </summary>
    private Rigidbody rb;

    // Entity target LayerMask, layer mask of those that will trigger unique effect
    [SerializeField]
    private LayerMask entityTargetLayerMask;

    /// <summary>
    /// Speed of projectile
    /// </summary>
    [SerializeField]
    private float projectileSpeedForce = 1;

    /// <summary>
    /// Damage done to entity on direct target
    /// </summary>
    private float directDamage = 60;

    private void Awake()
    {
        if (!rb)
            rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        //reset and set projectile velocity
        rb.velocity = Vector3.zero;
        rb.AddForce(transform.forward * projectileSpeedForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        // if target hit is included in layer Mask
        if (entityTargetLayerMask == (entityTargetLayerMask | (1 << other.gameObject.layer)))
        {
            Debug.Log("Damage target!");
            other.GetComponent<Entity>().AddHealth(-directDamage);
        }

        gameObject.SetActive(false);


    }


}
