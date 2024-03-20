using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/ButtonTypeConstant")]
public class ButtonTypeConstant : ScriptableObject
{
    public ButtonType ButtonType => Enum.Parse<ButtonType>(this.name);
}
