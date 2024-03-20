using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/GroundData")]
public class GroundData : ScriptableObject
{
    // Public properties.
    public string GroundId => this.name;

    // Public fields.
    public AudioClip groundFootstepSound;
}
