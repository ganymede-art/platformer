using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.script;
using UnityEngine.Serialization;

namespace Assets.script
{
    [System.Serializable]
    public struct GameItemData
    {
        public string type;
        public string group;
        public string code;

        public bool Equals(GameItemData other)
        {
            return Equals(other, this);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var objectToCompareWith = (GameItemData)obj;

            return objectToCompareWith.type == type 
                && objectToCompareWith.group == group
                && objectToCompareWith.code == code;

        }

        public static bool operator ==(GameItemData c1, GameItemData c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(GameItemData c1, GameItemData c2)
        {
            return !c1.Equals(c2);
        }

        public override int GetHashCode()
        {
            var calculation = type + group + code;
            return calculation.GetHashCode();
        }

        public string GetTypeAndGroup()
        {
            return type + "_" + group;
        }
    }

    [System.Serializable]
    public struct GameMusicData
    {
        [FormerlySerializedAs("code")]
        public string code;
        [FormerlySerializedAs("audio_clip")]
        public AudioClip audioClip;
        [FormerlySerializedAs("is_loop")]
        public bool isLoop;
    }



}
