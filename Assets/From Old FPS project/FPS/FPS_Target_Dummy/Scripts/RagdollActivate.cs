using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollActivate : MonoBehaviour {



    [SerializeField]
    private List<Collider> _colliders;

    public List<Rigidbody> _rigidbodies;

    private int _componentCount;

    private bool _activated;


    // Use this for initialization
    void Start()
    {         
        _colliders = new List<Collider>(GetComponentsInChildren<Collider>());
        _rigidbodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());



        for (int i = _colliders.Count - 1; i >= 0; --i)
        {
            if (_colliders[i].transform.parent == null)
            {
                _rigidbodies.RemoveAt(i);
                _colliders.RemoveAt(i);
                _componentCount = _colliders.Count;
                continue;
                // continue = finsih this cycle and go to the next on
            }
            //_colliders[i].enabled = false;
            _colliders[i].isTrigger = true;
            _rigidbodies[i].isKinematic = true;
        }

        _activated = false;

        // forr + tab = create reverse for loop (Change i-- to --i)



        //transform.GetComponent<Collider>().enabled = true;


    }
	
	


    /// <summary>
    /// Activate ragdoll
    /// </summary>
    public void Activate()
    {
        //Debug.Log("FART");
        for (int i = _componentCount - 1; i >= 0; --i)
        {
            //_colliders[i].enabled = true;
            _colliders[i].isTrigger = false;
            _rigidbodies[i].isKinematic = false;
        }

       

        _activated = true;
    }

    /// <summary>
    ///  Deactivate ragdoll
    /// </summary>
    public void Deactivate()
    {
        for (int i = _componentCount - 1; i >= 0; --i)
        {
            //_colliders[i].enabled = false;
            _colliders[i].isTrigger = true;
            _rigidbodies[i].isKinematic = true;
        }

        

        _activated = false;
    }



    public void AddExplosiveForce(float explosionForce, Vector3 exlposionPosition, float explosionRadius, float upwardModifier, ForceMode forceMode)
    {

        if (!_activated)
            _activated = true;

        for (int i = _colliders.Count - 1; i >= 0; --i)
        {
            _rigidbodies[i].AddExplosionForce(explosionForce, exlposionPosition, explosionRadius, upwardModifier, forceMode);
        }
    }


}
