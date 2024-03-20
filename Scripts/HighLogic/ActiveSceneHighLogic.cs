using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveSceneHighLogic : MonoBehaviour
{
    // Private fields.
    private GameObject cachedPlayerObject;
    private Player cachedPlayer;
    private GameObject cachedCamcorderObject;
    private Camcorder cachedCamcorder;

    private Dictionary<GameObject, GroundData> groundDatas;
    private Dictionary<GameObject, HitboxData> hitboxDatas;

    private Dictionary<GameObject, IInteractable> interactables;
    private Dictionary<GameObject, IKeyItemUsable> keyItemUsables;

    // Public properties.
    public static ActiveSceneHighLogic G => GameHighLogic.G?.ActiveSceneHighLogic;

    public GameObject CachedPlayerObject
    {
        get
        {
            if (cachedPlayerObject == null)
                cachedPlayerObject = GameObject.FindObjectOfType<Player>().gameObject;
            return cachedPlayerObject;
        }
    }

    public Player CachedPlayer
    {
        get
        {
            if (cachedPlayer == null)
                cachedPlayer = GameObject.FindObjectOfType<Player>();
            return cachedPlayer;
        }
    }

    public GameObject CachedCamcorderObject
    {
        get
        {
            if (cachedCamcorderObject == null)
                cachedCamcorderObject = GameObject.FindObjectOfType<Camcorder>().gameObject;
            return cachedCamcorderObject;
        }
    }

    public Camcorder CachedCamcorder
    {
        get
        {
            if (cachedCamcorder == null)
                cachedCamcorder = GameObject.FindObjectOfType<Camcorder>();
            return cachedCamcorder;
        }
    }

    public Dictionary<GameObject, GroundData> GroundDatas => groundDatas;
    public Dictionary<GameObject, HitboxData> HitboxDatas => hitboxDatas;

    public Dictionary<GameObject, IInteractable> Interactables => interactables;
    public Dictionary<GameObject, IKeyItemUsable> KeyItemUsables => keyItemUsables;

    private void Awake()
    {
        groundDatas = new Dictionary<GameObject, GroundData>();
        hitboxDatas = new Dictionary<GameObject, HitboxData>();

        interactables = new Dictionary<GameObject, IInteractable>();
        keyItemUsables = new Dictionary<GameObject, IKeyItemUsable>();

        // Subscribe to necessary events.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        cachedCamcorder = null;
        cachedPlayer = null;
        cachedCamcorderObject = null;
        cachedPlayerObject = null;

        groundDatas.Clear();
        hitboxDatas.Clear();
        interactables.Clear();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ActiveSceneHighLogic))]
public class ActiveSceneHighLogicEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var trigger = (ActiveSceneHighLogic)target;

        if (EditorApplication.isPlaying)
        {
            GUILayout.Label($"Ground Datas: {trigger.GroundDatas.Count}");
        }
    }
}
#endif