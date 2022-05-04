using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Script;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Script
{
    class EventSetActorAnimator : MonoBehaviour, IEventController
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
            master = GameMasterController.Global;
        }

        public GameObject GetNextEventSource()
        {
            return nextEventSource;
        }

        public string GetEventType()
        {
            return GameConstants.EVENT_TYPE_SET_ACTOR_ANIMATOR;
        }

        public string GetEventDescription()
        {
            return GetEventType();
        }

        public void StartEvent(GameEvent gameEvent)
        {
            if(actorAnimator == null)
                actorAnimator = actorObject.GetComponentInChildren<Animator>();

            actorAnimator.ResetAllAnimatorTriggers();
            actorAnimator.SetTrigger(trigger);
        }

        public void UpdateEvent(GameEvent gameEvent) { }

        public bool GetIsEventComplete(GameEvent gameEvent)
        {
            return true;
        }

        public bool GetIsUpdateComplete(GameEvent gameEvent)
        {
            return GetIsEventComplete(gameEvent);
        }

        public void FinishEvent(GameEvent gameEvent) { }

        public void ResetEvent(GameEvent gameEvent) { }

        private void OnDrawGizmos()
        {
            EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource);
        }
    }
}
