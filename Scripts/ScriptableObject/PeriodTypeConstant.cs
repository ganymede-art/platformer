using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/PeriodTypeConstant")]
public class PeriodTypeConstant : ScriptableObject
{
    public PeriodType PeriodType => Enum.Parse<PeriodType>(this.name);
}
