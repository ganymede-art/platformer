using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/NavigationNodeTypeConstant")]
public class NavigationNodeTypeConstant : ScriptableObject
{
    public NavigationNodeType NavigationNodeType => Enum.Parse<NavigationNodeType>(this.name);
}
