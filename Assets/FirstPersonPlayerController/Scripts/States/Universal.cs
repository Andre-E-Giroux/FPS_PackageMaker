using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universal : BaseState
{
    protected PlayerSM _sm;

  
    private float _verticalCameraRotClamp;



    private Transform _playerTransform;


    public Universal(string name, PlayerSM stateMachine) : base(name, stateMachine)
    {
        _sm = (PlayerSM)stateMachine;
        _playerTransform = _sm.transform;
        

        Cursor.lockState = CursorLockMode.Locked; 
        //to hide the curser
        Cursor.visible = false;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();


        //CamerRotationX();
        // CameraRotationY();

        _sm.GroundedCheck();
        _sm.JumpAndGravity();
        _sm.UniversalMove(_sm.grounded);


        //crouch
        if (Input.GetKeyDown(KeyCode.C) && _sm.grounded)
        {
            // check if they can uncrouch

            if (!_sm.isCrouching)
            {

                _sm.isCrouching = !_sm.isCrouching;
                stateMachine.ChangeState(_sm.crouchIdleState);
            }
            else if (_sm.isCrouching)
            {
                _sm.isCrouching = !_sm.isCrouching;
                stateMachine.ChangeState(_sm.standingIdleState);
            }
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        CameraRotation();
    }

    public static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void CameraRotation()
    {
        float deltaTimeMultiplier = _sm.IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

        //Don't multiply mouse input by Time.deltaTime
        // pitch
        _sm.cinemachineTargetPitch += ((Input.GetAxis("Mouse Y") * _sm.invertedMouseVal) * _sm.mouseSensitivetyY) * _sm.rotationSpeed * deltaTimeMultiplier;
        //yaw
        _sm.rotationVelocity = (Input.GetAxis("Mouse X") * _sm.mouseSensitivetyX) * _sm.rotationSpeed * deltaTimeMultiplier;

        // clamp our pitch rotation
        _sm.cinemachineTargetPitch = ClampAngle(_sm.cinemachineTargetPitch, _sm.BottomClamp, _sm.TopClamp);

        // Update Cinemachine camera target pitch
        _sm.cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_sm.cinemachineTargetPitch, 0.0f, 0.0f);

        // rotate the player left and right
        _playerTransform.Rotate(Vector3.up * _sm.rotationVelocity);
        
    }

    /*
    private void CamerRotationX()
    {
        // mouse X
        float mouseX = Input.GetAxis("Mouse X");

        float rotationNumberX = mouseX * _sm.mouseSensitivetyX;
        Vector3 cameraRotation = _cameraTransform.localEulerAngles;
        Vector3 playerRotation = _playerTransform.rotation.eulerAngles;

        cameraRotation.z = 0;

        playerRotation.y += rotationNumberX;

        _cameraTransform.localRotation = Quaternion.Euler(cameraRotation);
        _playerTransform.rotation = Quaternion.Euler(playerRotation);

    }

    public void CameraRotationY()
    {
        float mouseY = Input.GetAxis("Mouse Y");

        float rotationNumberY = mouseY * _sm.mouseSensitivetyY;

        Vector3 cameraRotation = _cameraTransform.rotation.eulerAngles;

        cameraRotation.x -= rotationNumberY;

        cameraRotation.z = 0;

        _verticalCameraRotClamp -= rotationNumberY;


        if (_verticalCameraRotClamp > 90)
        {
            _verticalCameraRotClamp = 90;
            cameraRotation.x = 90;
        }

        else if (_verticalCameraRotClamp < -90)
        {
            _verticalCameraRotClamp = -90;
            cameraRotation.x = 270;
        }

        _cameraTransform.rotation = Quaternion.Euler(cameraRotation);
    }

    */
    public virtual float GetWeaponAccuracyModifer()
    {
        return 1;
    }
}
