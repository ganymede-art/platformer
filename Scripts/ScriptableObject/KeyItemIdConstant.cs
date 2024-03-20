using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/KeyItemIdConstant")]
public class KeyItemIdConstant : ScriptableObject
{
    public string KeyItemId => this.name;
}
