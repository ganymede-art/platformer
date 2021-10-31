using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.script;
using UnityEngine.Serialization;
using static Assets.script.GameConstants;

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
        [FormerlySerializedAs("audio_clip")]
        public AudioClip audioClip;
        [FormerlySerializedAs("is_loop")]
        public bool isLoop;
    }

    [System.Serializable]
    public class GameEvent
    {
        public GameState gameState;

        public GameObject controllerSource;
        public IEventController controller;

        public GameObject previousControllerSource;
        public IEventController previousController;

        public bool isStarted;
        public bool isFinished;

        public float runningTimer;
        public float processTimer;

        public GameEvent(GameState gameState, GameObject controllerSource)
        {
            this.gameState = gameState;
            this.controllerSource = controllerSource;
            controller = controllerSource.GetComponent<IEventController>();

            previousControllerSource = null;
            previousController = null;

            isStarted = false;
            isFinished = false;

            runningTimer = 0.0f;
            processTimer = 0.0f;
        }
    }


}
