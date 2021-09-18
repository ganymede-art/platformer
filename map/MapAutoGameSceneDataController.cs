using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;

public class MapAutoGameSceneDataController : MonoBehaviour
{
    GameMasterController master;
    public GameSceneData scene_data;

    void Start()
    {
        master = GameMasterController.GetMasterController();
        master.user_interface_controller.ui_controller_game.StartSceneDataDisplay();
    }
}
