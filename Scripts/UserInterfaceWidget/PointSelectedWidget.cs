using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointSelectedWidget : MonoBehaviour, IUserInterfaceWidget
{
    // Private fields.
    private UserInterfaceWidgetStatus status;

    // Public properties.
    public string WidgetId => widgetId;
    public UserInterfaceWidgetType WidgetType => UserInterfaceWidgetType.PointSelected;
    public UserInterfaceWidgetStatus Status => status;
    public GameObject WidgetGameObject => gameObject;
    public bool IsAutomatic => isAutomatic;

    // Public fields.
    [Header("Widget Attributes")]
    [ContextMenuItem("Set Random Id", "SetRandomWidgetId")]
    public string widgetId;
    public bool isAutomatic;

    [Header("Pointer Attributes")]
    public GameObject pointerObject;

    // Context methods.
    private void SetRandomWidgetId() => widgetId = Guid.NewGuid().ToString();

    private void Awake()
    {
        status = UserInterfaceWidgetStatus.Disabled;
    }

    private void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == null)
        {
            pointerObject.SetActive(false);
        }
        else
        {
            pointerObject.SetActive(true);
            pointerObject.transform.position = EventSystem.current.currentSelectedGameObject.transform.position;
        }
    }

    public void BeginWidget(Dictionary<string, object> args = null)
    {
        gameObject.SetActive(true);
        status = UserInterfaceWidgetStatus.Enabled;
    }

    public void EndWidget()
    {
        gameObject.SetActive(false);
        status = UserInterfaceWidgetStatus.Disabled;
    }

    public void RefreshWidget(Dictionary<string, object> args = null)
    {
        
    }
}
