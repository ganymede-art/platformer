using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/VoxData")]
public class VoxData : ScriptableObject
{
    public Sprite voxSprite;
    public AudioClip[] voxSounds;
    public float minPitch;
    public float maxPitch;
}
