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
    public struct GameItemData
    {
        public string item_group;
        public string item_code;
    }

    [System.Serializable]
    public struct GameSceneData
    {
        [System.NonSerialized] public string scene_name;
        public string scene_description;

        public static GameSceneData GetDefault()
        {
            var data = new GameSceneData();
            data.scene_name = string.Empty;
            data.scene_description = string.Empty;
            return data;
        }
    }


}
