using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script
{
    class EventSetPlayerAbility : MonoBehaviour, IEventController
    {
        public GameObject nextEventSource;

        public bool canNowAttack;
        public bool canNowCrouchJump;
        public bool canNowDive;
        public bool canNowWaterDive;
        public bool canNowWaterJump;
        public bool canNowDoubleJump;

        public void FinishEvent(GameEvent gameEvent) { }

        public string GetEventType()
        {
            return GameConstants.EVENT_TYPE_SET_PLAYER_ABILITY;
        }

        public string GetEventDescription()
        {
            return GetEventType();
        }

        public bool GetIsEventComplete(GameEvent gameEvent)
        {
            return true;
        }

        public bool GetIsGameEventComplete(GameEvent gameEvent)
        {
            return true;
        }

        public bool GetIsProcessComplete(GameEvent gameEvent)
        {
            return true;
        }

        public GameObject GetNextEventSource()
        {
            return nextEventSource;
        }

        public void ProcessEvent(GameEvent gameEvent) { }

        public void StartEvent(GameEvent gameEvent)
        {
            if (canNowAttack)
                GameMasterController.Global.playerController.canAttack = true;

            if (canNowCrouchJump)
                GameMasterController.Global.playerController.canCrouchJump = true;

            if (canNowDive)
                GameMasterController.Global.playerController.canDive = true;

            if(canNowWaterDive)
                GameMasterController.Global.playerController.canWaterDive = true;

            if (canNowWaterJump)
                GameMasterController.Global.playerController.canWaterJump = true;

            if (canNowDoubleJump)
                GameMasterController.Global.playerController.canDoubleJump = true;
        }

        public void ResetEvent(GameEvent gameEvent) { }

        private void OnDrawGizmos()
        {
            EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource);
        }
    }
}
