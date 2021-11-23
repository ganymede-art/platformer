using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script
{
    class ReplacerGameVarInt : MonoBehaviour, IReplacerController
    {
        public string gameVarIntName;

        public string GetReplacement()
        {
            return GameDataController.Global.GetGameVarInt(gameVarIntName).ToString();
        }

        public object GetReplacementValue()
        {
            return GameDataController.Global.GetGameVarInt(gameVarIntName);
        }
    }
}
