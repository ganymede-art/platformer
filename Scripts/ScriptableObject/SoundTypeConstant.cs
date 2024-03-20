using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/SoundTypeConstant")]
public class SoundTypeConstant : ScriptableObject
{
    public SoundType soundType => Enum.Parse<SoundType>(this.name);
}
