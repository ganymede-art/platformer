using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script
{
    [CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/RandomDropData")]
    public class RandomDropData : ScriptableObject
    {
        public GameObject randomDropPrefab;
        public float randomDropChance;
    }
}
