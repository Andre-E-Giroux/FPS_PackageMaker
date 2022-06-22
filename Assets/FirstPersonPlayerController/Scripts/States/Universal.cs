using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universal : BaseState
{
    protected PlayerSM _sm;

    private float _mouseSensitivetyX = 1f;
    private float _mouseSensitivetyY = 1f;
    private float _verticalCameraRotClamp;

    private Transform _cameraTransform;
    private Transform _playerTransform;


    public Universal(string name, PlayerSM stateMachine) : base(name, stateMachine)
    {
        _sm = (PlayerSM)stateMachine;
        _playerTransform = _sm.transform;
        _cameraTransform = _sm.transform.GetChild(0);

        Cursor.lockState = CursorLockMode.Locked; 
        //to hide the curser
        Cursor.visible = false;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();


        CamerRotationX();
        CameraRotationY();



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


    private void CamerRotationX()
    {
        // mouse X
        float mouseX = Input.GetAxis("Mouse X");

        float rotationNumberX = mouseX * _mouseSensitivetyX;
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

        float rotationNumberY = mouseY * _mouseSensitivetyY;

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


    public virtual float GetWeaponAccuracyModifer()
    {
        return 1;
    }
}
