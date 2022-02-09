using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using static Assets.script.GameConstants;

public class UserInterfaceGameController : MonoBehaviour
{
    // constants.

    const float DESC_APPEAR_TIME = 1F;
    const float DESC_DISAPPEAR_TIME = 5F;
    const float DESC_FINISH_TIME = 6F;

    GameMasterController master;

    // root.

    GameObject uiObject;
    private GameObject uiHealthContainerObject;

    private GameObject uiSceneTitleObject;
    private TextMeshProUGUI uiSceneTitleText;
    private GameObject uiSceneSubtitleObject;
    private TextMeshProUGUI uiSceneSubtitleText;

    private GameObject[] uiHealthObjects;

    private GameObject uiCountBObject;
    private TextMeshProUGUI uiCountBText;

    private GameObject uiCountAmmoObject;
    private TextMeshProUGUI uiCountAmmoText;

    // scene description variables.

    private bool isDisplayingTitle;
    private float titleDisplayTimer;
    private Color titleDisplayColour;

    void Start()
    {
        master = this.GetComponentInParent<GameMasterController>();

        // add event hooks.

        master.GameStateChange += ChangeGameState;
        master.dataController.GameItemChange += GameItemChange;

        // scene description variables.

        isDisplayingTitle = false;
        titleDisplayTimer = 0.0F;
        titleDisplayColour = new Color(1, 1, 1, 0);

        // initialise ui.

        uiObject = this.gameObject;

        // container.

        uiHealthContainerObject = uiObject.transform.Find("ui_health_container").gameObject;

        // health objects.

        uiHealthObjects = new GameObject[20];

        for (int i = 0; i < uiHealthObjects.Length; i++)
        {
            uiHealthObjects[i]
                = uiHealthContainerObject.transform.Find("ui_health_" + i).gameObject;
        }

        // item objects.

        uiCountBObject = GameObject.Find("ui_count_b");
        uiCountBText = uiCountBObject.GetComponent<TextMeshProUGUI>();

        uiCountAmmoObject = GameObject.Find("ui_count_ammo");
        uiCountAmmoText = uiCountAmmoObject.GetComponent<TextMeshProUGUI>();

        // scene description.

        uiSceneTitleObject = uiObject.transform.Find("ui_scene_title").gameObject;
        uiSceneTitleText = uiSceneTitleObject.GetComponent<TextMeshProUGUI>();

        uiSceneSubtitleObject = uiObject.transform.Find("ui_scene_subtitle").gameObject;
        uiSceneSubtitleText = uiSceneSubtitleObject.GetComponent<TextMeshProUGUI>();

        uiObject.SetActive(false);
    }

    private void OnDestroy()
    {
        master.GameStateChange -= ChangeGameState;
    }

    void Update()
    {
        if (master.gameState != GAME_STATE_GAME)
            return;

        uiCountAmmoText.text = GamePlayerController.Global.ammo.ToString();

        for (int i = 0; i < uiHealthObjects.Length; i++)
        {
            uiHealthObjects[i].SetActive(master.playerController.health > i);
        }

        if (isDisplayingTitle)
        {
            titleDisplayTimer += Time.deltaTime;

            if (titleDisplayTimer >= DESC_FINISH_TIME)
            {
                uiSceneTitleObject.SetActive(false);
                uiSceneSubtitleObject.SetActive(false);
                isDisplayingTitle = false;
            }

            if(titleDisplayColour.a <= 1 
                && titleDisplayTimer <= DESC_APPEAR_TIME)
            {
                titleDisplayColour.a = Mathf.InverseLerp(0.0f, DESC_APPEAR_TIME, titleDisplayTimer);
            }

            if(titleDisplayColour.a >= 0 
                && titleDisplayTimer >= DESC_DISAPPEAR_TIME)
            {
                titleDisplayColour.a -= 0.01F;
            }

            uiSceneTitleText.color = titleDisplayColour;
            uiSceneSubtitleText.color = titleDisplayColour;
        }
        else
        {
            uiSceneTitleObject.SetActive(false);
            uiSceneSubtitleObject.SetActive(false);
        }
    }

    private void SetMenu()
    {
        uiObject.SetActive(true);

        uiCountBText.text = master.dataController.GetItemCountByType("b").ToString();
    }

    private void UnsetMenu()
    {
        uiObject.SetActive(false);
    }

    private void ChangeGameState(object sender, EventArgs e)
    {
        GameStateChangeEventArgs args = e as GameStateChangeEventArgs;

        if (args.gameState == GAME_STATE_GAME)
            SetMenu();
        else
            UnsetMenu();
    }

    private void GameItemChange(object sender, EventArgs e)
    {
        uiCountBText.text = master.dataController.GetItemCountByType("b").ToString();
    }

    public void SetSceneTitleDisplay(string sceneTitle, string sceneSubtitle)
    {
        uiSceneTitleObject.SetActive(true);
        uiSceneSubtitleObject.SetActive(true);
        uiSceneTitleText.text = sceneTitle;
        uiSceneSubtitleText.text = sceneSubtitle;

        isDisplayingTitle = true;

        titleDisplayColour.a = 0.0F;
        titleDisplayTimer = 0.0F;
    }
}
