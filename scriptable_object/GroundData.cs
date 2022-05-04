using Assets.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Script.GameConstants;

namespace Assets.Script
{
    [CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/AttributeGroundTypeData")]
    public class GroundData : ScriptableObject
    {
        public string groundType;
        public bool isGroundSlide;
        public AudioClip groundStepSound;
    }
}
