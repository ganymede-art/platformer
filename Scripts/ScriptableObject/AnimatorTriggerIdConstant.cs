using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/AnimatorTriggerIdConstant")]
public class AnimatorTriggerIdConstant : ScriptableObject
{
    public string AnimatorTriggerId => this.name;
}
