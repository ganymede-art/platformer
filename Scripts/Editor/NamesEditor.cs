using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Constants;

public class NamesEditor
{
    const string GREY_ICON_NAME = "sv_icon_dot0_pix16_gizmo";

    [MenuItem("Tools/1 Set Names")]
    public static void SetNames()
    {
        var gameObjects = GameObject.FindObjectsOfType<GameObject>();

        SetINameableNames(gameObjects);
        SetGreyIconNames(gameObjects);
        SetAddActionHighLogicTriggerNames();
        SetActionNames();
        SetSceneTriggerNames();
    }

    [MenuItem("Tools/1 Set Data")]
    public static void SetData()
    {
        SetItemData();
        SetVertexColourSamplerData();
    }

    private static void SetItemData()
    {
        var gameObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (var gameObject in gameObjects)
        {
            var trigger = gameObject.GetComponent<Item>();
            if (trigger == null)
                continue;
            trigger.itemId = Guid.NewGuid().ToString();
            PrefabUtility.RecordPrefabInstancePropertyModifications(trigger);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }

    private static void SetINameableNames(GameObject[] gameObjects)
    {
        foreach (var gameobject in gameObjects)
        {
            var trigger = gameobject.GetComponent<INameable>();
            if (trigger == null)
                continue;
            gameobject.name = trigger.GetName();
        }
    }

    private static void SetGreyIconNames(GameObject[] gameObjects)
    {
        foreach(var gameobject in gameObjects)
        {
            var iconName = EditorGUIUtility.GetIconForObject(gameobject)?.name;
            if (iconName == null)
                continue;
            if (iconName != GREY_ICON_NAME)
                continue;
            string prefabName = GetNameFromPrefab(gameobject);
            gameobject.name = prefabName;
        }
    }

    private static void SetAddActionHighLogicTriggerNames()
    {
        var gameObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (var gameObject in gameObjects)
        {
            var trigger = gameObject.GetComponent<AddActionHighLogicTrigger>();
            if (trigger == null)
                continue;
            gameObject.name = $"AddAction{trigger.actionId}";
        }
    }

    private static void SetActionNames()
    {
        var gameObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (var gameObject in gameObjects)
        {
            var action = gameObject.GetComponent<IAction>();
            if (action == null)
                continue;
            gameObject.name = action.ActionName;
        }
    }

    private static void SetSceneTriggerNames()
    {
        var gameObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach(var gameObject in gameObjects)
        {
            SetLoadNewSceneTriggerName(gameObject);
            SetDeathBarrierTriggerName(gameObject);

            var trigger = gameObject.GetComponent<InteractAddActionTrigger>();
            if (trigger == null)
                continue;
            if (trigger.highLogicTrigger == null)
                continue;
            gameObject.name = $"AddActionTrigger{trigger.highLogicTrigger.actionId}";
        }
    }

    private static void SetLoadNewSceneTriggerName(GameObject gameObject)
    {
        var lnsTrigger = gameObject.GetComponent<LoadNewSceneHighLogicTrigger>();
        if (lnsTrigger != null)
            gameObject.name = $"To{lnsTrigger.newSceneName}";
    }

    private static void SetDeathBarrierTriggerName(GameObject gameObject)
    {
        var trigger = gameObject.GetComponent<CollisionDeathBarrierTrigger>();
        if (trigger != null)
            gameObject.name = "DeathBarrier";
    }

    

    private static void SetVertexColourSamplerData()
    {
        var gameObjects = GameObject.FindObjectsOfType<GameObject>();

        var staticGameObject = gameObjects.FirstOrDefault(x => x.layer == SCENE_STATIC_IGNORE_CAMERA || x.layer == SCENE_STATIC);
        if (staticGameObject == null)
            return;
        var staticMeshFilter = staticGameObject.GetComponent<MeshFilter>();
        if (staticMeshFilter == null)
            return;

        foreach (var gameObject in gameObjects)
        {
            var trigger = gameObject.GetComponent<VertexColourSampler>();
            if (trigger == null)
                continue;
            trigger.sourceMeshFilter = staticMeshFilter;
            trigger.CalculateSourceColourIndex();

            PrefabUtility.RecordPrefabInstancePropertyModifications(trigger);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }

    private static string GetNameFromPrefab(GameObject gameObject)
    {
        var prefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(gameObject);
        if (prefab == null)
            return gameObject.name;
        return prefab.name;
    }
}