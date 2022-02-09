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

        public GameObject audioSourceObject;
        public AudioClip sound;

        private void Start()
        {
            if (audioSourceObject == null)
                audioSourceObject = gameObject;

            source = audioSourceObject.GetComponent<AudioSource>();

            if (source == null)
            {
                source = this.gameObject.AddComponent<AudioSource>();
                source.clip = sound;
                source.spatialBlend = 1.0F;
            }

            source.volume = source.volume * GameSettingsController.Global.volumeProp;
        }

        public void FinishEvent(GameEvent gameEvent) { }

        public string GetEventType()
        {
            return GameConstants.EVENT_TYPE_PLAY_SOUND;
        }

        public string GetEventDescription()
        {
            return GameConstants.EVENT_TYPE_PLAY_SOUND + ((audioSourceObject == null) ? "_null" : "_" + audioSourceObject.name);
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
            source.Play();
        }

        public void ResetEvent(GameEvent gameEvent) { }

        private void OnDrawGizmos()
        {
            EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource);
        }
    }
}
