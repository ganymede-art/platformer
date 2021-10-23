using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;

public class MapCameraModeTrigger : MonoBehaviour
{
    private GameObject playerCamera;

    public Transform fixedTransform;
    public bool isTracking;
    public bool isInstant;

    void Start()
    {
        playerCamera = GameMasterController.GlobalCameraObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GameConstants.TAG_PLAYER)
        {
            playerCamera.GetComponent<CameraController>()
                .SetFixedCamera(fixedTransform, isTracking, isInstant);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == GameConstants.TAG_PLAYER)
        {
            playerCamera.GetComponent<CameraController>().UnsetCamera();
        }
    }
}
