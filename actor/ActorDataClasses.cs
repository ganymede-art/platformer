using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.script;
using static Assets.script.GameConstants;

namespace Assets.script
{
    [System.Serializable]
    public class ActorData
    {
        public bool isInWater;
        public bool isSubmerged;
        public float waterYLevel;
        public GroundType groundType;

        public static ActorData GetDefault()
        {
            var adm = new ActorData();
            adm.isInWater = false;
            adm.isSubmerged = false;
            adm.waterYLevel = 0f;
            adm.groundType = GroundType.ground_default;
            return adm;
        }
    }
}
