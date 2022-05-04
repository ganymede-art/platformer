using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script
{
    public class EventLoadScene : MonoBehaviour, IEventController
    {
        public GameObject nextEventSource = null;
        public GameObject gameLoadSceneTriggerObject;

        public void FinishEvent(GameEvent gameEvent) { }

        public string GetEventDescription()
        {
            return GameConstants.EVENT_TYPE_LOAD_SCENE;
        }

        public string GetEventType()
        {
            return GameConstants.EVENT_TYPE_LOAD_SCENE;
        }

        public bool GetIsEventComplete(GameEvent gameEvent) { return true; }
        public bool GetIsUpdateComplete(GameEvent gameEvent) { return true; }

        public GameObject GetNextEventSource()
        {
            return nextEventSource;
        }

        public void ResetEvent(GameEvent gameEvent) { }

        public void StartEvent(GameEvent gameEvent)
        {
            var gameLoadSceneTrigger = gameLoadSceneTriggerObject
                .GetComponent<GameLoadSceneTrigger>();
            gameLoadSceneTrigger.StartLoadScene();
        }

        public void UpdateEvent(GameEvent gameEvent) { }

        private void OnDrawGizmos()
        {
            EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource);
        }
    }
}
