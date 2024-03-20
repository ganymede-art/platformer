using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/PropStatusConstant")]
public class PropStatusConstant : ScriptableObject
{
    public PropStatus PropStatus => Enum.Parse<PropStatus>(this.name);
}
