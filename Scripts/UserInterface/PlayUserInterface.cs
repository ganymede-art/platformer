using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class PlayUserInterface : MonoBehaviour, IUserInterface
{
    // Private fields.
    private UserInterfaceStatus activeStatus;
    private UserInterfaceStatus previousStatus;
    private float statusTimer;
    private IUserInterfaceWidget[] widgets;

    // Public properties.
    public UserInterfaceStatus Status => activeStatus;
    public IUserInterfaceWidget[] Widgets => widgets;

    private void Awake()
    {
        activeStatus = UserInterfaceStatus.Disabled;
        widgets = gameObject.GetComponentsInChildren<IUserInterfaceWidget>();
        foreach (var widget in widgets)
            widget.WidgetGameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(activeStatus == UserInterfaceStatus.BeginDisable)
        {
            if(statusTimer >= UI_BEGIN_DISABLE_INTERVAL)
            {
                ChangeStatus(UserInterfaceStatus.Disabled);
                return;
            }
        }

        statusTimer += Time.deltaTime;
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
        if(activeStatus == UserInterfaceStatus.BeginEnable)
        {
            gameObject.SetActive(true);
            foreach (var widget in widgets)
            {
                widget.WidgetGameObject.SetActive(true);
                if (!widget.IsAutomatic)
                    continue;
                widget.BeginWidget();
            }
        }
        else if(activeStatus == UserInterfaceStatus.BeginDisable)
        {
            foreach (var widget in widgets)
                widget.EndWidget();
        }
        else if(activeStatus == UserInterfaceStatus.Disabled)
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
}
