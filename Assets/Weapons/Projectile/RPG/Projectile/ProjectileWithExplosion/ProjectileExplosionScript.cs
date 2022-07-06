using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosionScript : ProjectileScript
{
    private ObjectPooler explosionObjectPooler;
    [SerializeField]
    private string explosionPoolerID = "pool_Explosions";

    private float maxExplosionDistance = 5.0f;
    private float maxExplosionDamage = 5.0f;


    private EntityList eList;

    private void Start()
    {
        if (!explosionObjectPooler)
            foreach (ObjectPooler op in GameObject.FindGameObjectWithTag("Pool").GetComponents<ObjectPooler>())
                if (op.poolerID == explosionPoolerID)
                    explosionObjectPooler = op;

        //SDebug.Log("hello");
        if (eList == null)
        {
            eList = FindObjectOfType<EntityList>();
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
   
        GameObject obj = explosionObjectPooler.GetPooledObject();

        obj.transform.position = transform.position;
        obj.SetActive(true);

        eList.CheckForExplosionDistance(transform, maxExplosionDistance, maxExplosionDamage);

        base.OnTriggerEnter(other);

    }
}
