using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoadSceneTrigger : MonoBehaviour
{
    public string scene;
    public string playerStartTransformName;
    public string cameraStartTransformName;
    public UserInterfaceTransitionData transitionData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartLoadScene()
    {
        GameLoadSceneController.Global.StartLoadGameScene
            (scene, playerStartTransformName, cameraStartTransformName, transitionData);
    }
}
