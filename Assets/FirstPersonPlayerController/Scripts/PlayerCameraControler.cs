using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControler : MonoBehaviour
{
    [SerializeField]
    private float _mouseSensitivetyX = 1f;
    [SerializeField]
    private float _mouseSensitivetyY = 1f;
    [SerializeField]
    private float _verticalCameraRotClamp = 0;

    [SerializeField]
    private Transform _cameraTransform;
    [SerializeField]
    private Transform _playerTransform;

    //Not implemented, attempted fix at camera stutter when straffing


    private void Update()
    {
        CamerRotationX();
        CameraRotationY();
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

}
