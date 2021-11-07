using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script
{ 
    public class EventPlayMusic : MonoBehaviour, IEventController
    {

        public GameObject nextEventSource;

        public GameMusicData musicData;

        public void FinishEvent(GameEvent gameEvent) { }

        public string GetEventType()
        {
            return GameConstants.EVENT_TYPE_PLAY_MUSIC;
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
            GameMasterController.GlobalMasterController.audioController.PlayMusic(musicData);
        }

        public void ResetEvent(GameEvent gameEvent) { }

        private void OnDrawGizmos()
        {
            EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource);
        }
    }

    
}
