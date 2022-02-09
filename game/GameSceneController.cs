using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using System;
using Assets.script;

public class GameSceneController : MonoBehaviour
{
    private static GameSceneController global;
    public static GameSceneController Global
    {
        get
        {
            if (global == null)
            {
                global = GameMasterController.Global.sceneController;
            }
            return global;
        }
    }

    // public fields.

    [NonSerialized] public string sceneGroup;
    [NonSerialized] public string sceneTitle;
    [NonSerialized] public string sceneSubtitle;

    [NonSerialized] public List<IInteractable> interactables;

    void Start()
    {
        interactables = new List<IInteractable>();

        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    private void SceneLoaded(Scene scene, LoadSceneMode load_scene_mode)
    {
        // clear existing data.

        sceneGroup = string.Empty;
        sceneTitle = string.Empty;
        sceneSubtitle = string.Empty;

        interactables.Clear();

        // get scene info, if it exists.

        InitialiseSceneInfo();
    }

    private void InitialiseSceneInfo()
    {
        var sceneInfoObject = GameObject.Find(GameConstants.NAME_GAME_SCENE_DATA);

        if (sceneInfoObject == null)
            return;

        var sceneInfo = sceneInfoObject.GetComponent<MapSceneInfo>();

        if (sceneInfo == null)
            return;

        sceneGroup = sceneInfo.sceneGroup;
        sceneTitle = sceneInfo.sceneTitle;
        sceneSubtitle = sceneInfo.sceneSubtitle;

        GameUserInterfaceController.Global.uiControllerGame.SetSceneTitleDisplay(sceneTitle, sceneSubtitle);
    }
}
