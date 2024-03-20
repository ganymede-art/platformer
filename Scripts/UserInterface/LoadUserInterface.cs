using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadUserInterface : MonoBehaviour, IUserInterface
{
    // Private fields.
    private UserInterfaceStatus status;
    private IUserInterfaceWidget[] widgets;

    // Public properties.
    public UserInterfaceStatus Status => status;
    public IUserInterfaceWidget[] Widgets => widgets;

    private void Awake()
    {
        status = UserInterfaceStatus.Disabled;
        widgets = gameObject.GetComponentsInChildren<IUserInterfaceWidget>();
        foreach (var widget in widgets)
            widget.WidgetGameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void BeginUserInterface(Dictionary<string, object> args = null)
    {
        gameObject.SetActive(true);
        status = UserInterfaceStatus.BeginEnable;
        foreach (var widget in widgets)
        {
            widget.WidgetGameObject.SetActive(false);
            if (!widget.IsAutomatic)
                continue;
            widget.BeginWidget();
        }
    }

    public void EndUserInterface()
    {
        status = UserInterfaceStatus.BeginDisable;
        foreach (var widget in widgets)
            widget.EndWidget();
    }
}
