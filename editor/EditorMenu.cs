using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Assets.script;
using System.Linq;

public class EditorMenu 
{
    [MenuItem("Tools/Set All Event Source Names")]
    public static void SetAllEventSourceNames()
    {
        var eventSources = GameObject.FindObjectsOfType<GameObject>();

        foreach(var eventSource in eventSources)
        {
            var evController = eventSource.GetComponent<IEventController>();

            if (evController == null)
                continue;

            eventSource.name = evController.GetEventDescription();
        }
    }

    [MenuItem("Tools/Set Selected Event Source Names")]
    public static void SetSelectedEventSourceNames()
    {
        var additionalEventSources = Selection.gameObjects;

        foreach (var eventSource in additionalEventSources)
        {
            var evController = eventSource.GetComponent<IEventController>();

            if (evController == null)
                continue;

            eventSource.name = evController.GetEventType();
        }
    }

    [MenuItem("Tools/Stack Selected Event Source Objects")]
    public static void StackSelectedEventSourceObjects()
    {
        var eventSources = Selection.gameObjects;

        GameObject currentEvent;
        Vector3 currentPosition;
        IEventController currentEvController;

        Vector3 offset = new Vector3(0, 0.5f, 0);

        foreach(var eventSource in eventSources)
        {
            currentEvent = eventSource;
            currentPosition = currentEvent.transform.position;
            currentEvController = currentEvent.GetComponent<IEventController>();

            while(currentEvController.GetNextEventSource() != null)
            {
                currentEvent = currentEvController.GetNextEventSource();
                currentEvent.transform.position = currentPosition += offset;
                currentPosition = currentEvent.transform.position;
                currentEvController = currentEvent.GetComponent<IEventController>();
            }
        }
    }
}
