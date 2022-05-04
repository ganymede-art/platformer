using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Script.GameConstants;

namespace Assets.Script
{
    [CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/DamageData")]
    public class DamageData : ScriptableObject
    {
        public int damageAmount;
        public bool isDamageInstant;
        public float horizontalForceMultiplier;
        public float verticalForceMultiplier;
    }
}
