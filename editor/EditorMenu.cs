using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Assets.Script;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class EditorMenu 
{
    [MenuItem("Tools/Create Scene Root Objects")]
    public static void CreateSceneRootObjects()
    {
        var rootObjectNames = new string[]
            {"scene_data"
            ,"ambient_events"
            ,"ambient_objects"
            ,"ambient_positions"
            ,"cutscene_events"
            ,"cutscene_objects"
            ,"cutscene_positions"
            ,"cutscene_triggers"
            ,"interactable_events"
            ,"interactable_objects"
            ,"interactable_positions"
            ,"map_lights"
            ,"map_load_objects"
            ,"map_triggers"
            ,"game_mobs"
            ,"game_npcs"
            ,"game_items"
            ,"map_objects"
            ,"map_common_props"
            ,"map_foliage_props"
            ,"map_dummy_objects"};
    
        foreach(string rootObjectName in rootObjectNames)
        {
            var rootObject = GameObject.Find(rootObjectName);
            if(rootObject == null)
            {
                var newObject = new GameObject(rootObjectName);
            }
        }
    }



    [MenuItem("Tools/Event Set All Source Names")]
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

    [MenuItem("Tools/Event Set Selected Source Names")]
    public static void SetSelectedEventSourceNames()
    {
        var additionalEventSources = Selection.gameObjects;

        foreach (var eventSource in additionalEventSources)
        {
            var evController = eventSource.GetComponent<IEventController>();

            if (evController == null)
                continue;

            eventSource.name =  evController.GetEventType();
        }
    }

    [MenuItem("Tools/Event Stack Selected Source Objects")]
    public static void StackSelectedEventSourceObjects()
    {
        var eventSources = Selection.gameObjects;

        GameObject currentEvent;
        Vector3 currentPosition;
        IEventController currentEvController;

        // offset for each event.
        var offset = new Vector3(0, 0.5f, 0);

        // store events already stacked to prevent infinite loops.
        var alreadyStackedEventSources = new List<GameObject>();

        foreach(var eventSource in eventSources)
        {
            currentEvent = eventSource;
            currentPosition = currentEvent.transform.position;
            currentEvController = currentEvent.GetComponent<IEventController>();
            
            while(currentEvController.GetNextEventSource() != null)
            {
                currentEvent = currentEvController.GetNextEventSource();

                if (alreadyStackedEventSources.Contains(currentEvent))
                    break;
                else
                    alreadyStackedEventSources.Add(currentEvent);

                currentEvent.transform.position = currentPosition += offset;
                currentPosition = currentEvent.transform.position;
                currentEvController = currentEvent.GetComponent<IEventController>();
            }
        }
    }

    [MenuItem("Tools/Item Set All Object Names")]
    public static void SetAllItemObjectNames()
    {
        var itemObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (var itemObject in itemObjects)
        {
            var itemController = itemObject.GetComponent<ItemController>();

            if (itemController == null)
                continue;

            string type = itemController.itemInfo.type;
            string group = itemController.itemInfo.group;
            string code = itemController.itemInfo.code;
            string itemName = $"item_{type}_{group}_{code}";
            itemObject.name = itemName;
        }
    }

    [MenuItem("Tools/Item Set All Object Data")]
    public static void SetAllItemObjectData()
    {
        var sceneInfoObject = GameObject.Find(GameConstants.NAME_GAME_SCENE_DATA);

        if (sceneInfoObject == null)
        {
            Debug.Log("No Scene Data.");
            return;
        }

        var sceneInfoController = sceneInfoObject.GetComponent<MapSceneInfo>();

        if (sceneInfoController == null)
        {
            Debug.Log("Missing Scene Data controller.");
            return;
        }

        var itemObjects = GameObject.FindObjectsOfType<GameObject>();

        int codeCount = 0;

        foreach (var itemObject in itemObjects)
        {
            var itemController = itemObject.GetComponent<ItemController>();

            if (itemController == null)
                continue;

            itemController.itemInfo.group = sceneInfoController.sceneGroup;
            itemController.itemInfo.code =  
                SceneManager.GetActiveScene().name + "_" + codeCount.ToString();

            codeCount++;
        }
    }

}

