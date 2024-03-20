using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Editor
{
    [MenuItem("Time/00:00")]
    static void ZeroAm() => SetPeriod(0.0F);
    [MenuItem("Time/01:00")]
    static void OneAm() => SetPeriod(1.0F);
    [MenuItem("Time/02:00")]
    static void TwoAm() => SetPeriod(2.0F);
    [MenuItem("Time/03:00")]
    static void ThreeAm() => SetPeriod(3.0F);
    [MenuItem("Time/04:00")]
    static void FourAm() => SetPeriod(4.0F);
    [MenuItem("Time/05:00")]
    static void FiveAm() => SetPeriod(5.0F);
    [MenuItem("Time/06:00")]
    static void SixAm() => SetPeriod(6.0F);
    [MenuItem("Time/07:00")]
    static void SevenAm() => SetPeriod(7.0F);
    [MenuItem("Time/08:00")]
    static void EightAm() => SetPeriod(8.0F);
    [MenuItem("Time/09:00")]
    static void NineAm() => SetPeriod(9.0F);
    [MenuItem("Time/10:00")]
    static void TenAm() => SetPeriod(10.0F);
    [MenuItem("Time/11:00")]
    static void ElevenAm() => SetPeriod(11.0F);
    [MenuItem("Time/12:00")]
    static void TwelvePm() => SetPeriod(12.0F);
    [MenuItem("Time/13:00")]
    static void OnePm() => SetPeriod(13.0F);
    [MenuItem("Time/14:00")]
    static void TwoPm() => SetPeriod(14.0F);
    [MenuItem("Time/15:00")]
    static void ThreePm() => SetPeriod(15.0F);
    [MenuItem("Time/16:00")]
    static void FourPm() => SetPeriod(16.0F);
    [MenuItem("Time/17:00")]
    static void FivePm() => SetPeriod(17.0F);
    [MenuItem("Time/18:00")]
    static void SixPm() => SetPeriod(18.0F);
    [MenuItem("Time/19:00")]
    static void SevenPm() => SetPeriod(19.0F);
    [MenuItem("Time/20:00")]
    static void EightPm() => SetPeriod(20.0F);
    [MenuItem("Time/21:00")]
    static void NinePm() => SetPeriod(21.0F);
    [MenuItem("Time/22:00")]
    static void TenPm() => SetPeriod(22.0F);
    [MenuItem("Time/23:00")]
    static void ElevenPm() => SetPeriod(23.0F);

    private static void SetPeriod(float hour)
    {
        var gameObjects = GameObject.FindObjectsOfType<GameObject>(includeInactive: true);

        PeriodType periodType = TimeHighLogic.GetPeriodFromHour(hour);

        foreach (var gameObject in gameObjects)
        {
            var triggers = gameObject.GetComponents<IPeriodObserver>();
            if (triggers == null)
                continue;
            foreach(var trigger in triggers)
                trigger.OnPeriodChanged(periodType);
        }

        foreach (var gameObject in gameObjects)
        {
            var triggers = gameObject.GetComponents<IHourObserver>();
            if (triggers == null)
                continue;
            foreach (var trigger in triggers)
                trigger.OnHourChanged(hour);
        }
    }

    [MenuItem("Tools/0 Create Materials For Textures")]
    static void CreateMaterials()
    {
        try
        {
            AssetDatabase.StartAssetEditing();
            var textures = Selection.GetFiltered(typeof(Texture), SelectionMode.Assets).Cast<Texture>();
            foreach (var tex in textures)
            {
                string path = AssetDatabase.GetAssetPath(tex);
                var directory = Path.GetDirectoryName(path);
                var filename = tex.name;

                if (filename.StartsWith("t_"))
                    filename = "m_" + tex.name.Substring(2);
                else
                    filename = $"m_{tex.name}";

                path = directory + "/" + filename + ".mat";

                if (AssetDatabase.LoadAssetAtPath(path, typeof(Material)) != null)
                {
                    Debug.LogWarning("Can't create material, it already exists: " + path);
                    continue;
                }
                var mat = new Material(Shader.Find("Project/Diffuse"));
                mat.mainTexture = tex;
                AssetDatabase.CreateAsset(mat, path);
            }
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
            AssetDatabase.SaveAssets();
        }
    }

    [MenuItem("Tools/0 Create Scene Root Objects")]
    static void CreateSceneRootObjects()
    {
        string[] rootObjectNames =
        {
            "ActionContainers",
            "ActionObjects",
            "ActionPositions",
            "ActionTriggers",
            "MapObjects",
            "MapTriggers",
            "MapLights",
            "MapAmbientObjects",
            "MapItems",
            "MapNpcs",
            "MapMobs",
            "MapNaturalProps",
            "MapStaticProps",
            "MapDynamicProps",
            "MapDummyObjects",
        };

        foreach (string rootObjectName in rootObjectNames)
        {
            var rootObject = GameObject.Find(rootObjectName);
            if (rootObject == null)
            {
                var newObject = new GameObject(rootObjectName);
            }
        }

        // Add components.
        var dummyObject = GameObject.Find("MapDummyObjects");
        var dummyAutoDestroy = dummyObject.GetComponent<AutoDestroy>();
        if (dummyAutoDestroy == null)
        {
            dummyAutoDestroy = dummyObject.AddComponent<AutoDestroy>();
            dummyAutoDestroy.destroyObject = dummyObject;
            dummyAutoDestroy.destroyInterval = 0.0F;
        }
    }
}
