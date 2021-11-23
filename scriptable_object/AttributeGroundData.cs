using Assets.script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.script.GameConstants;

namespace Assets.Script
{
    [CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/AttributeGroundTypeData")]
    public class AttributeGroundData : ScriptableObject
    {
        public string groundType;
        public bool isGroundSlide;

        public static AttributeGroundData GetDefault()
        {
            var data = new AttributeGroundData();
            data.groundType = GameConstants.GROUND_TYPE_DEFAULT;
            data.isGroundSlide = false;

            return data;
        }
    }
}
