using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.script;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.script
{
    class EventSetActorAnimatorController : MonoBehaviour, IEventController
    {
        private GameMasterController master;
        private Animator actorAnimator = null;

        [FormerlySerializedAs("next_event_source")]
        public GameObject nextEventSource = null;
        [FormerlySerializedAs("actor_object")]
        public GameObject actorObject = null;
        public string trigger = string.Empty;

        void Start()
        {
            master = GameMasterController.GetMasterController();
        }

        public GameObject GetNextEventSource()
        {
            return nextEventSource;
        }

        public string GetEventType()
        {
            return GameConstants.EVENT_TYPE_SET_ACTOR_ANIMATOR;
        }

        public void StartEvent()
        {
            if(actorAnimator == null)
                actorAnimator = actorObject.GetComponentInChildren<Animator>();

            actorAnimator.SetTrigger(trigger);
        }

        public void ProcessEvent() { }

        public bool GetIsEventComplete()
        {
            return true;
        }

        public bool GetIsProcessComplete()
        {
            return GetIsEventComplete();
        }

        public bool GetIsGameEventComplete()
        {
            return GetIsEventComplete();
        }

        public void FinishEvent() { }
    }
}
