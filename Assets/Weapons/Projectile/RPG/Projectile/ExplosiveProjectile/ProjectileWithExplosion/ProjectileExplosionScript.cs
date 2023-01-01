using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosionScript : ProjectileScript
{
    private ObjectPooler explosionObjectPooler;
    [SerializeField]
    private string explosionPoolerID = "pool_Explosions";

    [SerializeField]
    private float maxExplosionDistance = 5.0f;
    [SerializeField]
    private float maxExplosionDamage = 5.0f;
    [SerializeField]
    private float damageToDistanceModifier = 1;
    [SerializeField]
    private float explosiveForce = 15f;

    [SerializeField]
    private EntityListManager eList;

    public new  void Start()
    {

        if (!explosionObjectPooler)
            foreach (ObjectPooler op in GameObject.FindGameObjectWithTag("Pool").GetComponents<ObjectPooler>())
                if (op.poolerID == explosionPoolerID)
                    explosionObjectPooler = op;

        //SDebug.Log("hello");
        if (eList == null)
        {
            eList = FindObjectOfType<EntityListManager>();
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Debug.Log("Proj explosion has hit something");
        base.OnTriggerEnter(other);

        GameObject explosion = explosionObjectPooler.GetPooledObject();


        explosion.transform.position = transform.position;
        explosion.GetComponent<ExplosionScript>().SetExplosionValuesFromProjectile(maxExplosionDistance, maxExplosionDamage, damageToDistanceModifier, explosiveForce);
        explosion.SetActive(true);


        
    }
}
