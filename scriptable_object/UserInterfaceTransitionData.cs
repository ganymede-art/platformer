using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.script
{
    [CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/UserInterfaceTransitionData")]
    public class UserInterfaceTransitionData : ScriptableObject
    {
        public float transitionInterval;
        public Sprite overlaySprite;
        public Color overlayColour;
        public AudioClip transitionSound;
    }
}
