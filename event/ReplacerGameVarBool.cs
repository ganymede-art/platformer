using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script
{
    class ReplacerGameVarBool : MonoBehaviour, IReplacerController
    {
        public string gameVarBoolName;

        public string GetReplacement()
        {
            return GameDataController.Global.GetGameVarBool(gameVarBoolName).ToString();
        }

        public object GetReplacementValue()
        {
            return GameDataController.Global.GetGameVarBool(gameVarBoolName);
        }
    }
}
