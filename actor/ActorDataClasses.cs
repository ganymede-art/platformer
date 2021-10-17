using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.script;

namespace Assets.script
{
    [System.Serializable]
    public struct ActorDataManager
    {
        public bool is_in_water;
        public bool is_submerged;
        public float water_y_level;

        public static ActorDataManager GetDefault()
        {
            var adm = new ActorDataManager();
            adm.is_in_water = false;
            adm.is_submerged = false;
            adm.water_y_level = 0f;
            return adm;
        }
    }

}
