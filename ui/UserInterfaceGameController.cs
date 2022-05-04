using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using static Assets.Script.GameConstants;

public class UserInterfaceGameController : MonoBehaviour
{
    GameMasterController master;

    // root.

    GameObject uiObject;
    private GameObject uiHealthContainerObject;

    private GameObject[] uiHealthObjects;

    private GameObject uiCountBObject;
    private TextMeshProUGUI uiCountBText;

    private GameObject uiCountAmmoObject;
    private TextMeshProUGUI uiCountAmmoText;

    private GameObject uiOxygenObject;


    void Start()
    {
        master = this.GetComponentInParent<GameMasterController>();

        // add event hooks.

        master.GameStateChange += ChangeGameState;
        master.dataController.GameItemChange += GameItemChange;

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

        // oxgen objects.

        uiOxygenObject = uiObject.transform.Find("ui_oxygen").gameObject;

        // item objects.

        uiCountBObject = GameObject.Find("ui_count_b");
        uiCountBText = uiCountBObject.GetComponent<TextMeshProUGUI>();

        uiCountAmmoObject = GameObject.Find("ui_count_ammo");
        uiCountAmmoText = uiCountAmmoObject.GetComponent<TextMeshProUGUI>();

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

        if (GameMasterController.GlobalPlayerController.behaviourWater.isFullSubmerged)
        {
            uiOxygenObject.SetActive(true);
            uiOxygenObject.transform.localScale = Vector3.one *
                ((float)GamePlayerController.Global.oxygen / (float)GamePlayerController.Global.maxOxygen);
        }
        else
        {
            uiOxygenObject.SetActive(false);
        }
    }

    private void SetMenu()
    {
        uiObject.SetActive(true);

        uiCountBText.text = master.dataController.GetItemCountByType(ITEM_TYPE_SECONDARY).ToString();
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
        uiCountBText.text = master.dataController.GetItemCountByType(ITEM_TYPE_SECONDARY).ToString();
    }
}
