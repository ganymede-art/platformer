using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;

public class MapAutoSceneTitleController : MonoBehaviour
{
    GameMasterController master;
    public string sceneTitle;
    public string sceneSubtitle;

    void Start()
    {
        master = GameMasterController.GetMasterController();
        master.user_interface_controller.ui_controller_game.SetSceneTitleDisplay(sceneTitle, sceneSubtitle);
    }
}
