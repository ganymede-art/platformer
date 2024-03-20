using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/DamageTypeConstant")]
public class DamageTypeConstant : ScriptableObject
{
    public DamageType DamageType => Enum.Parse<DamageType>(this.name);
}
