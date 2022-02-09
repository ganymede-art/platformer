using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using UnityEngine.Serialization;

public class MapLoadLevelTrigger : MonoBehaviour
{ 
    public GameObject gameLoadSceneTriggerObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag == GameConstants.TAG_PLAYER_OBJECT)
        {
            var gameLoadSceneTrigger = gameLoadSceneTriggerObject
                .GetComponent<GameLoadSceneTrigger>();
            gameLoadSceneTrigger.StartLoadScene();
        }
    }
}
