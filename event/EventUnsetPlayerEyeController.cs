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

        public void FinishEvent() {}

        public string GetEventType()
        {
            return GameConstants.EVENT_TYPE_UNSET_PLAYER_EYE;
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

        public void ProcessEvent() {}

        public void StartEvent()
        {
            var component = GameMasterController.GlobalPlayerObject.GetComponent<ActorEyeController>();
            component.UnsetEmote();
        }
    }
}
