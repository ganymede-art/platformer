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

        public void FinishEvent(GameEvent gameEvent) { }

        public string GetEventType()
        {
            return GameConstants.EVENT_TYPE_SET_PLAYER_ABILITY;
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

        public void ResetEvent(GameEvent gameEvent) { }

        private void OnDrawGizmos()
        {
            EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource);
        }
    }
}
