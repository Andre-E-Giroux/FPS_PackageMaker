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
    public StandingRunning standingRunningState;
    [HideInInspector]
    public Jumping jumpState;
    [HideInInspector]
    public CrouchingMoving crouchMovingState;
    [HideInInspector]
    public CrouchingIdle crouchIdleState;

    public CharacterController characterController;
    public MeshRenderer meshRenderer;

    public UnityEngine.UI.RawImage healthBar;

    public Transform cameraTransform;

    public GameObject cinemachineCameraTarget;

    public CapsuleCollider playerCollider;
    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool grounded = true;
    [Tooltip("Useful for rough ground")]
    public float groundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float groundedRadius = 0.5f;
    [Tooltip("What layers the character uses as ground")]
    public LayerMask groundLayers;

    [Header("Player speed attributes")]
    public float defaultSpeed = 4f;
    public float runningSpeed = 6f;
    public float crouchSpeed = 1.5f;
    public float jumpForce = 10f;

    [Header("Player Jump and gravity")]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;


    [Header("Player rotation")]
    public float rotationSpeed = 1.0f;
    [HideInInspector]
    public float rotationVelocity;

    [Header("Player mouse sensitivity")]
    public float mouseSensitivetyX = 1f;
    public float mouseSensitivetyY = 1f;

    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;





    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.1f;
    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    [Header("Misk attributes")]
    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    // falling terminal velocity
    private float _terminalVelocity = 53.0f;
    private float _speed;
    private float _verticalVelocity;


    //cinemachine
    public float cinemachineTargetPitch;


    /// <summary>
    /// -1 is standard vertical rotation
    /// 1 is inverted controls
    /// </summary>
    public int invertedMouseVal = -1;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 90.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -90.0f;

    // verify if mouse is the rotation device
    public bool IsCurrentDeviceMouse
    {
        get
        {
            #if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
				            return _playerInput.currentControlScheme == "KeyboardMouse";
            #else
                        return false;
            #endif
        }
    }

    [HideInInspector]
    public bool isCrouching = false;
    //consts
    private const float CAMERA_STANDING_HEIGHT = 1.63f;
    private const float CAMERA_CROUCHING_HEIGHT = 0.85f;
    private const float COLLIDER_STANDING_HEIGHT = 2;
    private const float COLLIDER_CROUCHING_HEIGHT = 1.22f;
    private const float COLLIDER_STANDING_CENTER = 1;
    private const float COLLIDER_CROUCHING_CENTER = 0.61F;

    [Header("Player reference")]
    [Tooltip("Player Entity reference")]
    public Entity playerEntity;
    [Tooltip("Player Weapon Interaction")]
    public WeaponInteraction wInteraction;

    public void Awake()
    {
        //initiate states
        standingIdleState = new StandingIdle(this);
        standingMovingState = new StandingMoving(this);
        standingRunningState = new StandingRunning(this);
        jumpState = new Jumping(this);
        crouchIdleState = new CrouchingIdle(this);
        crouchMovingState = new CrouchingMoving(this);


        playerEntity = gameObject.GetComponent<Entity>();
        grounded = true;

        wInteraction = GetComponent<WeaponInteraction>();

        playerCollider = GetComponent<CapsuleCollider>();
    }

    protected override BaseState GetInitialState()
    {
        return standingIdleState;
    }


    public void Move(float targetSpeed)
    {
        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(characterController.velocity.x, 0.0f, characterController.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = 1f;
        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        // normalise input direction
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical")).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) != Vector2.zero)
        {
            // move
            inputDirection = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
        }

        // move the player
        characterController.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }


    public void JumpAndGravity()
	{
		if (grounded)
		{
			// reset the fall timeout timer
			_fallTimeoutDelta = FallTimeout;

			// stop our velocity dropping infinitely when grounded
			if (_verticalVelocity < 0.0f)
			{
				_verticalVelocity = -2f;
			}

			// Jump
			if (Input.GetKeyDown(KeyCode.Space) && _jumpTimeoutDelta <= 0.0f)
			{
                Debug.Log("Jump FORCE!");
				// the square root of H * -2 * G = how much velocity needed to reach desired height
				_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

			}

			// jump timeout
			if (_jumpTimeoutDelta >= 0.0f)
			{
				_jumpTimeoutDelta -= Time.deltaTime;
			}
		}
		else
		{

            if(currentState != jumpState)
            {
                ChangeState(jumpState);
            }

            // reset the jump timeout timer
            _jumpTimeoutDelta = JumpTimeout;

			// fall timeout
			if (_fallTimeoutDelta >= 0.0f)
			{
				_fallTimeoutDelta -= Time.deltaTime;
			}

				
		}

		// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
		if (_verticalVelocity < _terminalVelocity)
		{
			_verticalVelocity += Gravity * Time.deltaTime;
		}

        characterController.Move(new Vector3(characterController.velocity.x, _verticalVelocity, characterController.velocity.z) * Time.deltaTime);

    }

    public void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
        grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
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
            cinemachineCameraTarget.transform.localPosition = new Vector3(cinemachineCameraTarget.transform.localPosition.x, CAMERA_CROUCHING_HEIGHT, cinemachineCameraTarget.transform.localPosition.z);
            //playerCollider.height = COLLIDER_CROUCHING_HEIGHT;
            //playerCollider.center = new Vector3(playerCollider.center.x, COLLIDER_CROUCHING_CENTER, playerCollider.center.z);
            characterController.height = COLLIDER_CROUCHING_HEIGHT;
            characterController.center = new Vector3(characterController.center.x, COLLIDER_CROUCHING_CENTER, characterController.center.z);

        }
        else
        {
            cinemachineCameraTarget.transform.localPosition = new Vector3(cinemachineCameraTarget.transform.localPosition.x, CAMERA_STANDING_HEIGHT, cinemachineCameraTarget.transform.localPosition.z);
            //playerCollider.height = COLLIDER_STANDING_HEIGHT;
            // playerCollider.center = new Vector3(playerCollider.center.x, COLLIDER_STANDING_CENTER, playerCollider.center.z);
            characterController.height = COLLIDER_STANDING_HEIGHT;
            characterController.center = new Vector3(characterController.center.x, COLLIDER_STANDING_CENTER, characterController.center.z);

        }
    }
}
