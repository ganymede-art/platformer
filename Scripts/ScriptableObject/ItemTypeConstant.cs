using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/ItemTypeConstant")]
public class ItemTypeConstant : ScriptableObject
{
    public ItemType ItemType => Enum.Parse<ItemType>(this.name);
}
