using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/HighLogicStateIdConstant")]
public class HighLogicStateIdConstant : ScriptableObject
{
    public HighLogicStateId highLogicStateId => Enum.Parse<HighLogicStateId>(this.name);
}
