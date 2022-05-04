using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using static Assets.Script.GameConstants;

public class MapCameraModeTrigger : MonoBehaviour
{
    const float TRANSITION_SPEED_MIN = 1.0F;
    const float TRANSITION_SPEED_DEFAULT = 2.0F;

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
        if (GameMasterController.Global.gameState != GAME_STATE_GAME)
            return;

        if (other.transform.root.tag == GameConstants.TAG_PLAYER_OBJECT)
        {
            playerCamera.GetComponent<CameraController>()
                .SetFixedCamera(fixedTransform, isTracking, isInstant, transitionSpeed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameMasterController.Global.gameState != GAME_STATE_GAME)
            return;

        if (other.transform.root.tag == GameConstants.TAG_PLAYER_OBJECT)
        {
            playerCamera.GetComponent<CameraController>().UnsetCamera();
        }
    }
}
