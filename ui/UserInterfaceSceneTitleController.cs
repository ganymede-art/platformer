using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Assets.Script.GameConstants;

public class UserInterfaceSceneTitleController : MonoBehaviour
{
    // constants.

    const float DESC_APPEAR_TIME = 1F;
    const float DESC_DISAPPEAR_TIME = 5F;
    const float DESC_FINISH_TIME = 6F;

    // ui vars.

    GameObject uiObject;

    private GameObject uiSceneTitleObject;
    private TextMeshProUGUI uiSceneTitleText;
    private GameObject uiSceneSubtitleObject;
    private TextMeshProUGUI uiSceneSubtitleText;

    private bool isDisplayingTitle;
    private float titleDisplayTimer;
    private Color titleDisplayColour;

    void Start()
    {
        // scene description variables.

        isDisplayingTitle = false;
        titleDisplayTimer = 0.0F;
        titleDisplayColour = new Color(1, 1, 1, 0);

        // initialise ui.

        uiObject = this.gameObject;

        // scene description.

        uiSceneTitleObject = uiObject.transform.Find("ui_scene_title").gameObject;
        uiSceneTitleText = uiSceneTitleObject.GetComponent<TextMeshProUGUI>();

        uiSceneSubtitleObject = uiObject.transform.Find("ui_scene_subtitle").gameObject;
        uiSceneSubtitleText = uiSceneSubtitleObject.GetComponent<TextMeshProUGUI>();

        uiObject.SetActive(false);
    }

    void Update()
    {
        if (GameMasterController.Global.gameState != GAME_STATE_GAME)
        {
            uiSceneTitleObject.SetActive(false);
            uiSceneSubtitleObject.SetActive(false);
            return;
        }

        uiSceneTitleObject.SetActive(true);
        uiSceneSubtitleObject.SetActive(true);

        if (isDisplayingTitle)
        {
            titleDisplayTimer += Time.deltaTime;

            if (titleDisplayTimer >= DESC_FINISH_TIME)
            {
                isDisplayingTitle = false;
            }

            if (titleDisplayColour.a <= 1
                && titleDisplayTimer <= DESC_APPEAR_TIME)
            {
                titleDisplayColour.a = Mathf.InverseLerp(0.0f, DESC_APPEAR_TIME, titleDisplayTimer);
            }

            if (titleDisplayColour.a >= 0
                && titleDisplayTimer >= DESC_DISAPPEAR_TIME)
            {
                titleDisplayColour.a -= 0.01F;
            }

            uiSceneTitleText.color = titleDisplayColour;
            uiSceneSubtitleText.color = titleDisplayColour;
        }
        else
        {
            UnsetMenu();
        }
    }

    private void SetMenu()
    {
        uiObject.SetActive(true);
    }

    private void UnsetMenu()
    {
        uiObject.SetActive(false);
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

        SetMenu();
    }
}
