using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script
{
    class EventSetPlayerEye : MonoBehaviour, IEventController
    {
        public GameObject nextEventSource;

        public int eyeEmoteIndex;

        public void FinishEvent(GameEvent gameEvent) { }

        public string GetEventType()
        {
            return GameConstants.EVENT_TYPE_SET_PLAYER_EYE;
        }

        public string GetEventDescription()
        {
            return GetEventType();
        }

        public bool GetIsEventComplete(GameEvent gameEvent)
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
            var component = GameMasterController.GlobalPlayerObject.GetComponentInChildren<ActorEyeController>();
            component.SetEmote(eyeEmoteIndex);
        }

        public void ResetEvent(GameEvent gameEvent) { }

        private void OnDrawGizmos()
        {
            EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource);
        }
    }
}
