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

    [SerializeField]
    private EntityList eList;

    public new  void Start()
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
        if (!other.isTrigger)
        {
            Debug.Log("TriggerHit");
            GameObject obj = explosionObjectPooler.GetPooledObject();

            Debug.Log(obj);

            obj.transform.position = transform.position;
            obj.SetActive(true);

            Debug.Log(eList);

            eList.CheckForExplosionDistance(transform.position, maxExplosionDistance, maxExplosionDamage);

            base.OnTriggerEnter(other);

        }
    }
}
