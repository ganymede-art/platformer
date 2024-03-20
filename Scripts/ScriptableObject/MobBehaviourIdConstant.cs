using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/MobBehaviourIdConstant")]
public class MobBehaviourIdConstant : ScriptableObject
{
    public MobBehaviourId mobBehaviourId => Enum.Parse<MobBehaviourId>(this.name);
}
