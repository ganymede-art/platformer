using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/VariableIdConstant")]
public class VariableIdConstant : ScriptableObject
{
    public string VariableId => this.name;
}
