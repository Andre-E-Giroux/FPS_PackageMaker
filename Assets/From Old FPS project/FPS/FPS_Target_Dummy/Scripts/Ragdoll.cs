using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour {


    private Animator _animator;
    private RagdollActivate _ragdollActivate;
    private Collider _collider;
    private Rigidbody _rigidbody;


    public bool ragdollIsActive = false;

    //private DummyDamageHandler_TF tf_ddh;



    // Use this for initialization
    void Start () {
        _animator = GetComponent<Animator>();
        _ragdollActivate = GetComponent<RagdollActivate>();
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();

        //tf_ddh = GetComponent<DummyDamageHandler_TF>();
    }
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            EnableRagdoll();
        }
        
	}


    public void EnableRagdoll()
    {
        //tf_ddh.enabled = false;
        ragdollIsActive = true;
        _ragdollActivate.Activate();
        _animator.enabled = false;
        _collider.enabled = false;
        _rigidbody.isKinematic = true;

    }

    public void DisableRagdoll()
    {
       // tf_ddh.enabled = true;
        ragdollIsActive = false;
        _ragdollActivate.Deactivate();
        _animator.enabled = true;
        _collider.enabled = true;
        _rigidbody.isKinematic = false;
    }



}
