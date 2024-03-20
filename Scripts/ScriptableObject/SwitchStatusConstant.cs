using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/SwitchStatusConstant")]
public class SwitchStatusConstant : ScriptableObject
{
    public SwitchStatus SwitchStatus => Enum.Parse<SwitchStatus>(this.name);
}
