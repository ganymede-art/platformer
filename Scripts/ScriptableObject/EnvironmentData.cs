using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/EnvironmentData")]
public class EnvironmentData : ScriptableObject
{
    [Header("Sky Attributes")]
    public Color ambientLightColour;

    [Header("Fog Attributes")]
    public bool isFogEnabled;
    public Color fogColour;
    public float fogDensity;

}
