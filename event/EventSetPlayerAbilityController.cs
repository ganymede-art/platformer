using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script
{
    class EventSetPlayerAbilityController : MonoBehaviour, IEventController
    {
        public GameObject nextEventSource;

        public bool canNowAttack;
        public bool canNowCrouchJump;
        public bool canNowDive;
        public bool canNowWaterDive;
        public bool canNowWaterJump;

        public void FinishEvent()
        {
            
        }

        public string GetEventType()
        {
            return GameConstants.EVENT_TYPE_SET_PLAYER_ABILITY;
        }

        public bool GetIsEventComplete()
        {
            return true;
        }

        public bool GetIsGameEventComplete()
        {
            return true;
        }

        public bool GetIsProcessComplete()
        {
            return true;
        }

        public GameObject GetNextEventSource()
        {
            return nextEventSource;
        }

        public void ProcessEvent() { }

        public void StartEvent()
        {
            if (canNowAttack)
                GameMasterController.GlobalMasterController.playerController.canAttack = true;

            if (canNowCrouchJump)
                GameMasterController.GlobalMasterController.playerController.canCrouchJump = true;

            if (canNowDive)
                GameMasterController.GlobalMasterController.playerController.canDive = true;

            if(canNowWaterDive)
                GameMasterController.GlobalMasterController.playerController.canWaterDive = true;

            if (canNowWaterJump)
                GameMasterController.GlobalMasterController.playerController.canWaterJump = true;
        }
    }
}
