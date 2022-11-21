using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollScript : MonoBehaviour
{
    /// <summary>
    /// Information on character accessories of a ragdol (ex: sword, gun, helmet, pencil, etc)
    /// </summary>
    public struct ChildParentConnection
    {
        public Transform child;

        public Vector3 childOriginLocalPosition;

        public Quaternion childOriginLocalRotation;

        public Behaviour[] behavioursActiveOnOrphan;

        public Transform parent;
    }

    /// <summary>
    /// Array that holds structs of a parent and child (Sword and hand)
    /// </summary>
    public ChildParentConnection[] extraItemsAndOwners;

    public Transform[] extraItems;

    [SerializeField]
    private List<Collider> _ragdollColliders;
    [SerializeField]
    private List<Rigidbody> _ragDollRigidbodies;

    private Animator _animator;
    private Collider _collider;
    private Rigidbody _rigidbody;

    private bool _isRagdollActivated;


    /// <summary>
    /// Call this function to enable the ragdoll
    /// </summary>
    /// <param name="activate">Is the ragdoll to be acitvated</param>
    public void SetActiveRagdoll(bool activate)
    {
        _animator.enabled = !activate;
        _collider.enabled = !activate;

        _rigidbody.isKinematic = activate;
        for (int i = 0; i < _ragdollColliders.Count; i++)
        {
            _ragdollColliders[i].isTrigger = !activate;
            _ragDollRigidbodies[i].isKinematic = !activate;
        }
    }

    /// <summary>
    /// Call this function to have the accessories of a character to be seperate of the main character. (ex: sword)
    /// </summary>
    /// <param name="toBeConnected">True: the accessories to be reconnected tot the character, False: the accessories will split from the character</param>
    public void SetExtraObjectConnection(bool toBeConnected)
    {

        if (extraItemsAndOwners.Length < 1)
            return;

        for (int i = 0; i < extraItemsAndOwners.Length; i++)
        {
            // return extra items to their parents
            if (toBeConnected)
            {
                extraItemsAndOwners[i].child.parent = extraItemsAndOwners[i].parent;
                extraItemsAndOwners[i].child.localPosition = extraItemsAndOwners[i].childOriginLocalPosition;
                extraItemsAndOwners[i].child.localRotation = extraItemsAndOwners[i].childOriginLocalRotation;

                foreach (Behaviour behaviour in extraItemsAndOwners[i].behavioursActiveOnOrphan)
                    behaviour.enabled = false;
            }
            else
            {
                extraItemsAndOwners[i].child.parent = null;

                foreach (Behaviour behaviour in extraItemsAndOwners[i].behavioursActiveOnOrphan)
                    behaviour.enabled = true;
            }
        }

          
    }

   

    /// <summary>
    /// Set the orrigin of all accessories as the current rot and position
    /// </summary>
    public void SetAccessorieOrigins()
    {
        if (extraItemsAndOwners == null)
            return;

        for (int i = 0; i < extraItemsAndOwners.Length; i++)
        {
            extraItemsAndOwners[i].childOriginLocalPosition = extraItemsAndOwners[i].child.localPosition;
            extraItemsAndOwners[i].childOriginLocalRotation = extraItemsAndOwners[i].child.localRotation;
        }
    }

    /// <summary>
    /// Initialize the ragdoll with basic data and references
    /// </summary>
    public void InitializeRagdoll()
    {

        extraItemsAndOwners = new ChildParentConnection[extraItems.Length];

        for(int i = 0; i < extraItems.Length; i++)
        {
            extraItemsAndOwners[i].child = extraItems[i];
            extraItemsAndOwners[i].parent = extraItems[i].parent;
        }
        SetAccessorieOrigins();

        // first rigid body is ignored, wut?! Look for fix/update 2021 build
        _ragDollRigidbodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
        _ragdollColliders = new List<Collider>(GetComponentsInChildren<Collider>());

        // components taht affects the ragdoll without being connected to it
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();

        for (int i = 0; i < _ragDollRigidbodies.Count; i++)
        {

            if (_ragDollRigidbodies[i].transform.parent == null)
            {
               // Debug.Log("No parent, removed: " + _ragDollRigidbodies[i].name);
                _ragDollRigidbodies.RemoveAt(i);
                i--;
            }
            else
            {
               // Debug.Log("Has set for: " + _ragDollRigidbodies[i].name);
                _ragDollRigidbodies[i].isKinematic = true;
            }
        }

        for (int i = 0; i < _ragdollColliders.Count; i++)
        {

            if (_ragdollColliders[i].transform.parent == null)
            {
                _ragdollColliders.RemoveAt(i);
                i--;
            }
            else
            {
                _ragdollColliders[i].isTrigger = true;
            }
        }

        _isRagdollActivated = false;
    }

    /// <summary>
    /// Addn explosive force to the ragdoll
    /// </summary>
    /// <param name="explosionForce">Force of the explosion</param>
    /// <param name="exlposionPosition">Origin of the explosion</param>
    /// <param name="explosionRadius">The radius of the explosion</param>
    /// <param name="upwardModifier">Modifier to give explosion more vertical for, for more flying ragdolls</param>
    /// <param name="forceMode">The force mode of the explosion</param>
    public void AddExplosiveForce(float explosionForce, Vector3 exlposionPosition, float explosionRadius, float upwardModifier, ForceMode forceMode)
    {

        if (!_isRagdollActivated)
            _isRagdollActivated = true;

        for (int i = _ragDollRigidbodies.Count - 1; i >= 0; --i)
        {
            _ragDollRigidbodies[i].AddExplosionForce(explosionForce, exlposionPosition, explosionRadius, upwardModifier, forceMode);
        }
    }

}
