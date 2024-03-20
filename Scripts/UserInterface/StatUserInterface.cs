using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatUserInterface : MonoBehaviour, IUserInterface
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
    public Button resumeButton;
    public Button loadButton;
    public Button quitButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(OnResumeButtonClick);
        loadButton.onClick.AddListener(OnLoadGameButtonClick);
        quitButton.onClick.AddListener(OnQuitGameButtonClick);

        activeStatus = UserInterfaceStatus.Disabled;
        widgets = gameObject.GetComponentsInChildren<IUserInterfaceWidget>();
        foreach (var widget in widgets)
            widget.WidgetGameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (activeStatus == UserInterfaceStatus.Disabled)
            return;

        if(activeStatus == UserInterfaceStatus.BeginEnable)
        {
            if (!UserInterfaceStatics.AreAllWidgetsEnabled(widgets))
                return;

            activeStatus = UserInterfaceStatus.Enabled;
        }
        else if(activeStatus == UserInterfaceStatus.BeginDisable)
        {
            if (!UserInterfaceStatics.AreAllWidgetsDisabled(widgets))
                return;

            activeStatus = UserInterfaceStatus.Disabled;
            gameObject.SetActive(false);
        }

        // Exit.

        if(activeStatus == UserInterfaceStatus.Enabled 
            && InputHighLogic.G.IsStartPressed 
            && !InputHighLogic.G.WasStartPressed)
        {
            StateHighLogic.G.ChangeState(HighLogicStateId.Play);
            return;
        }
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
            eventSystem.SetSelectedGameObject(resumeButton.gameObject);
            resumeButton.Select();
        }
        else if (activeStatus == UserInterfaceStatus.BeginDisable)
        {
            activeStatus = UserInterfaceStatus.BeginDisable;
            foreach (var widget in widgets)
                widget.EndWidget();
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(null);
        }
        else if (activeStatus == UserInterfaceStatus.Disabled)
        {
            gameObject.SetActive(false);
        }
    }

    private void EndStatus() { }

    public void OnResumeButtonClick()
    {
        StateHighLogic.G.ChangeState(HighLogicStateId.Play);
    }

    public void OnLoadGameButtonClick()
    {
        PersistenceHighLogic.G.LoadPersistence();
    }

    public void OnQuitGameButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void BeginUserInterface(Dictionary<string, object> args = null)
    {
        ChangeStatus(UserInterfaceStatus.BeginEnable);
    }

    public void EndUserInterface()
    {
        ChangeStatus(UserInterfaceStatus.BeginDisable);
    }
}
