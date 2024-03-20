using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class ActionStatics
{
    public static string GetTextWithStaticReplacers(string text)
    {
        return text;
    }

    public static string GetTextWithDynamicReplacers(string text, IReplacer[] replacers)
    {
        for (int i = 0; i < replacers.Length; i++)
            text = text.Replace($"%[{i}]", replacers[i].ReplacementValue);
        
        return text;
    }

    public static IReplacer[] GetReplacersFromObjects(GameObject[] replacerObjects)
    {
        var replacers = new IReplacer[replacerObjects.Length];

        for (int i = 0; i < replacerObjects.Length; i++)
            replacers[i] = replacerObjects[i].GetComponent<IReplacer>();

        return replacers;
    }
}
