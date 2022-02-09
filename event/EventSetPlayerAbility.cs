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
        public bool canNowFireProjectile;

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

        public bool IsGameEventComplete(GameEvent gameEvent)
        {
            return true;
        }

        public bool GetIsUpdateComplete(GameEvent gameEvent)
        {
            return true;
        }

        public GameObject GetNextEventSource()
        {
            return nextEventSource;
        }

        public void UpdateEvent(GameEvent gameEvent) { }

        public void StartEvent(GameEvent gameEvent)
        {
            if (canNowAttack)
                GamePlayerController.Global.canAttack = true;

            if (canNowCrouchJump)
                GamePlayerController.Global.canCrouchJump = true;

            if (canNowDive)
                GamePlayerController.Global.canDive = true;

            if(canNowWaterDive)
                GamePlayerController.Global.canWaterDive = true;

            if (canNowWaterJump)
                GamePlayerController.Global.canWaterJump = true;

            if (canNowDoubleJump)
                GamePlayerController.Global.canDoubleJump = true;

            if (canNowFireProjectile)
                GamePlayerController.Global.canFireProjectile = true;
        }

        public void ResetEvent(GameEvent gameEvent) { }

        private void OnDrawGizmos()
        {
            EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource);
        }
    }
}
