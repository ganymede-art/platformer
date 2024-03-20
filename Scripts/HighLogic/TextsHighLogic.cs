using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using static Constants;

public class TextsHighLogic : MonoBehaviour
{
    // Private fields.
    private TextAsset[] textAssets;
    private Dictionary<string, string> texts;

    // Public properties.
    public static TextsHighLogic G => GameHighLogic.G.TextsHighLogic;

    private void Awake()
    {
        textAssets = Resources.LoadAll<TextAsset>(RESOURCE_FOLDER_TEXTS);
        texts = new Dictionary<string, string>();
        var deserialiser = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        foreach(var textAsset in textAssets)
        {
            var localisationYaml = deserialiser.Deserialize<LocalisationYaml>(textAsset.text);

            foreach (var text in localisationYaml.texts)
                texts[text.Key] = text.Value;
        }
    }

    public string GetText(string textId)
    {
        string text = texts.GetValueOrDefault(textId);
        return text;
    }

    private class LocalisationYaml
    {
        public Dictionary<string, string> texts;
    }
}


