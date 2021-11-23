using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class UserInterfaceGameController : MonoBehaviour
{
    // constants.

    const float DESC_APPEAR_TIME = 1f;
    const float DESC_DISAPPEAR_TIME = 5f;
    const float DESC_FINISH_TIME = 6f;

    GameMasterController master;

    // root.

    GameObject uiObject;
    private GameObject uiHealthContainerObject;

    private GameObject uiSceneTitleObject;
    private TextMeshProUGUI uiSceneTitleText;
    private GameObject uiSceneSubtitleObject;
    private TextMeshProUGUI uiSceneSubtitleText;

    private GameObject[] uiHealthObjects;

    private GameObject uiItemBasicCountObject;
    private TextMeshProUGUI uiItemBasicCountText;

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
        titleDisplayTimer = 0.0f;
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

        uiItemBasicCountObject = GameObject.Find("ui_count_b");
        uiItemBasicCountText = uiItemBasicCountObject.GetComponent<TextMeshProUGUI>();

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
        if (master.gameState != GameState.Game)
            return;

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
                titleDisplayColour.a -= 0.01f;
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

        uiItemBasicCountText.text = master.dataController.GetItemCountByType("b").ToString();
    }

    private void UnsetMenu()
    {
        uiObject.SetActive(false);
    }

    private void ChangeGameState(object sender, EventArgs e)
    {
        GameStateChangeEventArgs args = e as GameStateChangeEventArgs;

        if (args.gameState == GameState.Game)
            SetMenu();
        else
            UnsetMenu();
    }

    private void GameItemChange(object sender, EventArgs e)
    {
        uiItemBasicCountText.text = master.dataController.GetItemCountByType("b").ToString();
    }

    public void SetSceneTitleDisplay(string sceneTitle, string sceneSubtitle)
    {
        uiSceneTitleObject.SetActive(true);
        uiSceneSubtitleObject.SetActive(true);
        uiSceneTitleText.text = sceneTitle;
        uiSceneSubtitleText.text = sceneSubtitle;

        isDisplayingTitle = true;

        titleDisplayColour.a = 0.0f;
        titleDisplayTimer = 0.0f;
    }
}
