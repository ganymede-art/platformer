using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Script.GameConstants;

namespace Assets.Script
{
    [CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/MusicData")]
    public class MusicData : ScriptableObject
    {
        public string code;
        public AudioClip audioClip;
        public bool isLoop;
    }
}
