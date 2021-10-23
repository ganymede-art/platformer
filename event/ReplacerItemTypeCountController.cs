using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using UnityEngine.Serialization;

public class ReplacerItemTypeCountController : MonoBehaviour, IReplacerController
{
    GameMasterController master;
    [FormerlySerializedAs("item_type")]
    public string itemType;

    public string GetReplacement()
    {
        return master.dataController.GetItemCountByType(itemType).ToString();
    }

    public object GetReplacementValue()
    {
        return master.dataController.GetItemCountByType(itemType);
    }

    void Start()
    {
        master = GameMasterController.GlobalMasterController;
    }

    
}
