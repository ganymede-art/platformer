using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Script;
using UnityEngine.Serialization;
using static Assets.Script.GameConstants;

namespace Assets.Script
{
    [System.Serializable]
    public struct GameItemInfo
    {
        public string type;
        public string group;
        public string code;

        public bool Equals(GameItemInfo other)
        {
            return Equals(other, this);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var objectToCompareWith = (GameItemInfo)obj;

            return objectToCompareWith.type == type 
                && objectToCompareWith.group == group
                && objectToCompareWith.code == code;

        }

        public static bool operator ==(GameItemInfo c1, GameItemInfo c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(GameItemInfo c1, GameItemInfo c2)
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
    public class GameEvent
    {
        public string eventGuid;

        public string gameState;

        public GameObject controllerSource;
        public IEventController controller;

        public GameObject previousControllerSource;
        public IEventController previousController;

        public bool isStarted;
        public bool isFinished;

        public float runningTimer;
        public float processTimer;

        public GameEvent(string eventGuid, string gameState, GameObject controllerSource)
        {
            this.eventGuid = eventGuid;
            this.gameState = gameState;
            this.controllerSource = controllerSource;
            controller = controllerSource.GetComponent<IEventController>();

            previousControllerSource = null;
            previousController = null;

            isStarted = false;
            isFinished = false;

            runningTimer = 0.0F;
            processTimer = 0.0F;
        }
    }


}
