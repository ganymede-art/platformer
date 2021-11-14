using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;

public class MapAutoSceneTitle : MonoBehaviour
{
    GameMasterController master;
    public string sceneTitle;
    public string sceneSubtitle;

    void Start()
    {
        master = GameMasterController.Global;
        master.userInterfaceController.uiControllerGame.SetSceneTitleDisplay(sceneTitle, sceneSubtitle);
    }
}
