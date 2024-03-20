using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/MusicData")]
public class MusicData : ScriptableObject
{
    public AudioClip musicAudioClip;
    public string musicGroup;
}
