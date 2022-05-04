using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using Assets.Script;

public class GroundDataController : MonoBehaviour
{
    public GroundData groundData;

    private void Start()
    {
        #if UNITY_EDITOR
        Debug.Log("[AttributeGroundController] Registering.");
        #endif

        GameSceneController.Global.groundDataObjects.Add(gameObject, groundData);  
    }

    private void OnDestroy()
    {
        GameSceneController.Global.groundDataObjects.Remove(gameObject);
    }
}
