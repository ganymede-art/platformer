using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using UnityEngine.Serialization;

public class MapLoadLevelTrigger : MonoBehaviour
{
    public string scene = string.Empty;
    [FormerlySerializedAs("player_start_transform")]
    public string playerStartTransform = string.Empty;
    [FormerlySerializedAs("camera_start_transform")]
    public string cameraStartTransform = string.Empty;

    public UserInterfaceTransitionData transitionData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GameConstants.TAG_PLAYER)
        {
            GameLoadLevelController.Global.StartLoadLevel(scene, playerStartTransform, cameraStartTransform, transitionData);
        }
    }
}
