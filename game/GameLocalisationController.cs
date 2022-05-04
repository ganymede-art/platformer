using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.IO;

public class GameLocalisationController : MonoBehaviour
{
    private static GameLocalisationController global;
    public static GameLocalisationController Global
    {
        get
        {
            if (global == null)
            {
                global = GameMasterController.Global.localisationController;
            }
            return global;
        }
    }

    [NonSerialized] public Dictionary<string, string> locs;
    public TextAsset[] localisationTexts;

    void Start()
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        locs = new Dictionary<string, string>();

        foreach (var localisationText in localisationTexts)
        {
            var localisationInfo = deserializer.Deserialize<LocalisationInfo>(localisationText.text);

            foreach (var key in localisationInfo.locs.Keys)
            {
                locs.Add(key, localisationInfo.locs[key]);
            }
        }
    }

}

public class LocalisationInfo
{
    public Dictionary<string, string> locs;
}
