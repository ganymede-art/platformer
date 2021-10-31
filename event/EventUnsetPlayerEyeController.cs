using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script
{
    public class EventUnsetPlayerEyeController : MonoBehaviour, IEventController
    {
        public GameObject nextEventSource;

        public void FinishEvent(GameEvent gameEvent) {}

        public string GetEventType()
        {
            return GameConstants.EVENT_TYPE_UNSET_PLAYER_EYE;
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

        public void ProcessEvent(GameEvent gameEvent) {}

        public void StartEvent(GameEvent gameEvent)
        {
            var component = GameMasterController.GlobalPlayerObject.GetComponentInChildren<ActorEyeController>();
            component.UnsetEmote();
        }

        public void ResetEvent(GameEvent gameEvent) { }

        private void OnDrawGizmos()
        {
            EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource);
        }
    }
}
