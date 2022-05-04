using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Script;
using static Assets.Script.GameConstants;
using Assets.Script;

namespace Assets.Script
{
    [System.Serializable]
    public class ActorData
    {
        public bool isInWater;
        public bool isSubmerged;
        public float waterYLevel;
        public GroundData groundData;

        public static ActorData GetDefault()
        {
            var adm = new ActorData();
            adm.isInWater = false;
            adm.isSubmerged = false;
            adm.waterYLevel = 0F;
            adm.groundData = null;
            return adm;
        }
    }
}
