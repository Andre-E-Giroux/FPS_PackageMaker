using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    public Transform playerCamera;


    public void SetCameraAsPlayerChild(bool toBeChilded)
    {
        if (toBeChilded)
            playerCamera.parent = transform;
        else
            playerCamera.parent = null;
    }

}
