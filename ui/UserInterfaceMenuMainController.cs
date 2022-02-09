using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Assets.script.GameConstants;

public class UserInterfaceMenuMainController : MonoBehaviour
{
    private GameObject uiObject;

    private GameObject uiStartButtonObject;
    private Button uiStartButton;
    private GameObject uiLoadButtonObject;
    private Button uiLoadButton;
    private GameObject uiSettingsButtonObject;
    private Button uiSettingsButton;
    private GameObject uiQuitButtonObject;
    private Button uiQuitButton;

    void Start()
    {
        // get ui components.

        uiObject = this.gameObject;

        uiStartButtonObject = GameObject.Find("ui_button_start");
        uiStartButton = uiStartButtonObject.GetComponent<Button>();
        uiStartButton.onClick.AddListener(OnStartButtonClick);

        uiLoadButtonObject = GameObject.Find("ui_button_load");
        uiLoadButton = uiLoadButtonObject.GetComponent<Button>();
        uiLoadButton.onClick.AddListener(OnLoadButtonClick);

        uiSettingsButtonObject = GameObject.Find("ui_button_settings");
        uiSettingsButton = uiSettingsButtonObject.GetComponent<Button>();
        uiSettingsButton.onClick.AddListener(OnSettingsButtonClick);

        uiQuitButtonObject = GameObject.Find("ui_button_quit");
        uiQuitButton = uiQuitButtonObject.GetComponent<Button>();
        uiQuitButton.onClick.AddListener(OnQuitButtonClick);

        // select initial button.

        var ev = EventSystem.current;
        ev.SetSelectedGameObject(uiStartButtonObject);
        uiStartButton.Select();
    }

    // behaviours

    public void OnStartButtonClick()
    {
        GameLoadSceneController.Global.StartLoadGameScene
            ("scene_test_1", "player_start_1", "camera_start_1");
    }

    public void OnLoadButtonClick()
    {
        GameDataController.Global.LoadData();
    }

    public void OnSettingsButtonClick()
    {
        GameLoadSceneController.Global.StartLoadMenuScene
            ("menu_settings",GAME_STATE_MENU_SETTINGS);
    }

    public void OnQuitButtonClick()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

}
