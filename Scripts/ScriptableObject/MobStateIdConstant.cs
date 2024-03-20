using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/MobStateIdConstant")]
public class MobStateIdConstant : ScriptableObject
{
    public MobStateId mobStateId => Enum.Parse<MobStateId>(this.name);
}
