using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.script
{
    [CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/UserInterfaceTransitionData", order = 1)]
    public class UserInterfaceTransitionData : ScriptableObject
    {
        public float transitionInterval;
        public Sprite overlaySprite;
        public Color overlayColour;
        public AudioClip transitionSound;
    }
}
