using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Constants;

public class MenuUserInterface : MonoBehaviour, IUserInterface
{
    // Private fields.
    private UserInterfaceStatus activeStatus;
    private UserInterfaceStatus previousStatus;
    private float statusTimer;
    private IUserInterfaceWidget[] widgets;

    // Public properties.
    public UserInterfaceStatus Status => activeStatus;
    public IUserInterfaceWidget[] Widgets => widgets;

    // Public fields.
    public Button newGameButton;
    public Button loadGameButton;
    public Button configGameButton;
    public Button quitGameButton;

    private void Awake()
    {
        newGameButton.onClick.AddListener(OnNewGameButtonClick);
        loadGameButton.onClick.AddListener(OnLoadGameButtonClick);
        configGameButton.onClick.AddListener(OnConfigGameButtonClick);
        quitGameButton.onClick.AddListener(OnQuitGameButtonClick);

        activeStatus = UserInterfaceStatus.Disabled;
        widgets = gameObject.GetComponentsInChildren<IUserInterfaceWidget>();
        foreach (var widget in widgets)
            widget.WidgetGameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void ChangeStatus(UserInterfaceStatus newStatus)
    {
        EndStatus();
        previousStatus = activeStatus;
        activeStatus = newStatus;
        statusTimer = 0.0F;
        BeginStatus();
    }

    private void BeginStatus()
    {
        if (activeStatus == UserInterfaceStatus.BeginEnable)
        {
            gameObject.SetActive(true);
            foreach (var widget in widgets)
            {
                widget.WidgetGameObject.SetActive(false);
                if (!widget.IsAutomatic)
                    continue;
                widget.WidgetGameObject.SetActive(true);
                widget.BeginWidget();
            }
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(newGameButton.gameObject);
            newGameButton.Select();
        }
        else if (activeStatus == UserInterfaceStatus.BeginDisable)
        {
            activeStatus = UserInterfaceStatus.BeginDisable;
            foreach (var widget in widgets)
                widget.EndWidget();
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(null);
            activeStatus = UserInterfaceStatus.Disabled;
            gameObject.SetActive(false);
        }
        else if (activeStatus == UserInterfaceStatus.Disabled)
        {
            gameObject.SetActive(false);
        }
    }

    private void EndStatus() { }

    public void BeginUserInterface(Dictionary<string, object> args = null)
    {
        ChangeStatus(UserInterfaceStatus.BeginEnable);
    }

    public void EndUserInterface()
    {
        ChangeStatus(UserInterfaceStatus.BeginDisable);
    }

    public void OnNewGameButtonClick()
    {
        LoadSceneHighLogic.G.LoadNewScene(NEW_GAME_SCENE_NAME, HighLogicStateId.Play);
    }

    public void OnLoadGameButtonClick()
    {
        PersistenceHighLogic.G.LoadPersistence();
    }

    public void OnConfigGameButtonClick()
    {

    }

    public void OnQuitGameButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
