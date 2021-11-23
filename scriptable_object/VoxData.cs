using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.script
{
    [CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/VoxData")]
    public class VoxData : ScriptableObject
    {
        [Header("Vox Attributes")]
        public Sprite voxSprite = null;
        public AudioClip[] voxSounds;
        public float voxSoundPitch = 1.0f;
    }
}
