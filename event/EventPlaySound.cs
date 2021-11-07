using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script
{
    class EventPlaySound : MonoBehaviour, IEventController
    {
        private AudioSource source;

        public GameObject nextEventSource;

        
        public AudioClip sound;

        private void Start()
        {
            source = this.gameObject.AddComponent<AudioSource>();
            source.clip = sound;
            source.spatialBlend = 1.0f;
        }

        public void FinishEvent(GameEvent gameEvent) { }

        public string GetEventType()
        {
            return GameConstants.EVENT_TYPE_PLAY_SOUND;
        }

        public string GetEventDescription()
        {
            return GameConstants.EVENT_TYPE_PLAY_SOUND + ((sound == null) ? "_null" : "_" + sound.name);
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
            source.Play();
        }

        public void ResetEvent(GameEvent gameEvent) { }

        private void OnDrawGizmos()
        {
            EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource);
        }
    }
}
