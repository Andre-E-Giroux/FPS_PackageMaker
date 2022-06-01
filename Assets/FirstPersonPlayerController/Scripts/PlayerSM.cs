using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSM : StateMachine
{
    [HideInInspector]
    public StandingIdle standingIdleState;
    [HideInInspector]
    public StandingMoving standingMovingState;
    [HideInInspector]
    public Jumping jumpState;
    [HideInInspector]
    public CrouchingMoving crouchMovingState;
    [HideInInspector]
    public CrouchingIdle crouchIdleState;
    //[HideInInspector]
    //public Attack attackState;

    public Rigidbody rb;
    public MeshRenderer meshRenderer;

    public UnityEngine.UI.RawImage healthBar;

    public Transform cameraTransform;
    public CapsuleCollider playerCollider;
    public bool grounded;

    public float speed = 4f;
    public float crouchSpeed = 1.5f;
    public float jumpForce = 10f;

   

    public float healthDecaySpeed = 4.0f;

    public bool isCrouching = false;

    [SerializeField]
    private const float CAMERA_STANDING_HEIGHT = 1.63f;
    [SerializeField]
    private const float CAMERA_CROUCHING_HEIGHT = 0.85f;

    [SerializeField]
    private const float COLLIDER_STANDING_HEIGHT = 2;
    [SerializeField]
    private const float COLLIDER_CROUCHING_HEIGHT = 1.22f;

    [SerializeField]
    private const float COLLIDER_STANDING_CENTER = 1;
    [SerializeField]
    private const float COLLIDER_CROUCHING_CENTER = 0.61F;

    public Entity playerEntity;


    public void Awake()
    {
        //initiate states
        standingIdleState = new StandingIdle(this);
        standingMovingState = new StandingMoving(this);
        jumpState = new Jumping(this);
        crouchIdleState = new CrouchingIdle(this);
        crouchMovingState = new CrouchingMoving(this);
        playerEntity = gameObject.GetComponent<Entity>();
        grounded = true;
       

        playerCollider = GetComponent<CapsuleCollider>();
    }

    protected override BaseState GetInitialState()
    {
        return standingIdleState;
    }

    public void AddHealth(float increase)
    {

    }


    /// <summary>
    /// Debuging State Maching
    /// </summary>
    private void OnGUI()
    {
        string content = currentState != null ? currentState.name : "(no current state)";
        GUILayout.Label($"<color='white'><size=40>{content}</size></color>");
    }

    public void CrouchPlayer(bool toCrouch)
    {
        if(toCrouch)
        {
            cameraTransform.position = new Vector3(cameraTransform.position.x, CAMERA_CROUCHING_HEIGHT, cameraTransform.position.z);
            playerCollider.height = COLLIDER_CROUCHING_HEIGHT;
            playerCollider.center = new Vector3(playerCollider.center.x, COLLIDER_CROUCHING_CENTER, playerCollider.center.z);
        }
        else
        {
            cameraTransform.position = new Vector3(cameraTransform.position.x, CAMERA_STANDING_HEIGHT, cameraTransform.position.z);
            playerCollider.height = COLLIDER_STANDING_HEIGHT;
            playerCollider.center = new Vector3(playerCollider.center.x, COLLIDER_STANDING_CENTER, playerCollider.center.z);

        }
    }

}
