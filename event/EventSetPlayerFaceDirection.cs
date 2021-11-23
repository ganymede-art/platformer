using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script
{ 
    public class EventSetPlayerFaceDirection : MonoBehaviour, IEventController
    {

        public GameObject nextEventSource;
        public GameObject faceDirectionTargetObject;

        public void FinishEvent(GameEvent gameEvent) { }

        public string GetEventType()
        {
            return GameConstants.EVENT_TYPE_SET_PLAYER_FACE_DIRECTION;
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
            GameMasterController.GlobalPlayerController.isCutsceneFaceDirection = true;
            GameMasterController.GlobalPlayerController.cutsceneFaceDirectionTargetObject = faceDirectionTargetObject;
        }

        public void ResetEvent(GameEvent gameEvent) { }

        private void OnDrawGizmos()
        {
            EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource, optionalObject1: faceDirectionTargetObject,optionalColour1: Color.cyan, optionalIcon1: "ev_facedir.png");
        }
    }

    
}
