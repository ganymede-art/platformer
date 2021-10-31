using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;

public class MapCameraModeTrigger : MonoBehaviour
{
    const float TRANSITION_SPEED_MIN = 1.0f;
    const float TRANSITION_SPEED_DEFAULT = 2.0f;

    private GameObject playerCamera;

    public Transform fixedTransform;
    public bool isTracking;
    public bool isInstant;
    public float transitionSpeed;

    void Start()
    {
        playerCamera = GameMasterController.GlobalCameraObject;

        if (transitionSpeed < TRANSITION_SPEED_MIN)
            transitionSpeed = TRANSITION_SPEED_DEFAULT;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GameConstants.TAG_PLAYER)
        {
            playerCamera.GetComponent<CameraController>()
                .SetFixedCamera(fixedTransform, isTracking, isInstant, transitionSpeed);
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
