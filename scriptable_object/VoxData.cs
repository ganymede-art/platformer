using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Assets.Script
{
    [CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/VoxData")]
    public class VoxData : ScriptableObject
    {
        [Header("Vox Attributes")]
        public Sprite voxSprite = null;
        public AudioClip[] voxSounds;
        [FormerlySerializedAs("voxSoundPitch")]
        public float minVoxSoundPitch = 1.0F;
        public float maxVoxSoundPitch = 1.25F;
    }
}
