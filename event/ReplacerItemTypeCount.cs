using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using UnityEngine.Serialization;

public class ReplacerItemTypeCount : MonoBehaviour, IReplacerController
{
    [FormerlySerializedAs("item_type")]
    public string itemType;

    public string GetReplacement()
    {
        return GameDataController.Global.GetItemCountByType(itemType).ToString();
    }

    public object GetReplacementValue()
    {
        return GameDataController.Global.GetItemCountByType(itemType);
    }
}
