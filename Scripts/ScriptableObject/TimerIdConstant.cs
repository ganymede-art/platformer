using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/TimerIdConstant")]
public class TimerIdConstant : ScriptableObject
{
    public string TimerId => this.name;
}
