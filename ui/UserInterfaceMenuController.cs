using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class UserInterfaceMenuController : MonoBehaviour
{
    GameMasterController master;

    // root.

    private GameObject ui_object;
    private GameObject ui_basic_count_object;
    private TextMeshProUGUI ui_basic_count_text;
    private GameObject ui_main_count_object;
    private TextMeshProUGUI ui_main_count_text;

    private GameObject ui_resume_object;
    private Button ui_resume_button;

    void Start()
    {
        master = this.GetComponentInParent<GameMasterController>();

        // add event hooks.

        master.GameStateChange += ChangeGameState;

        // initialise ui.

        ui_object = this.gameObject;

        ui_basic_count_object = GameObject.Find("ui_count_basic");
        ui_basic_count_text = ui_basic_count_object.GetComponent<TextMeshProUGUI>();
        ui_main_count_object = GameObject.Find("ui_count_main");
        ui_main_count_text = ui_main_count_object.GetComponent<TextMeshProUGUI>();

        ui_resume_object = GameObject.Find("ui_button_resume");
        ui_resume_button = ui_resume_object.GetComponent<Button>();

        ui_object.SetActive(false);
    }

    private void OnDestroy()
    {
        master.GameStateChange -= ChangeGameState;
    }

    private void SetMenu()
    {
        // update the total counts.

        ui_object.SetActive(true);

        int basic_count = master.data_controller.GetItemCountByType("basic");
        int main_count = master.data_controller.GetItemCountByType("main");

        ui_basic_count_text.text = basic_count.ToString();
        ui_main_count_text.text = main_count.ToString();

        var ev = EventSystem.current;
        ev.SetSelectedGameObject(ui_resume_object);
        ui_resume_button.Select();
    }

    private void UnsetMenu()
    {
        ui_object.SetActive(false);
    }


    private void ChangeGameState(object sender, EventArgs e)
    {
        GameStateChangeEventArgs args = e as GameStateChangeEventArgs;

        if (args.gameState == GameState.Menu)
            SetMenu();
        else
            UnsetMenu();
    }

    // behaviours

    public void ResumeGame()
    {
        master.ChangeState(master.gameStatePrevious);
    }

    public void LoadGame()
    {
        master.data_controller.LoadData();
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                         Application.Quit();
        #endif
    }
}
