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

        public void FinishEvent() { }

        public string GetEventType()
        {
            return GameConstants.EVENT_TYPE_PLAY_MUSIC;
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

        public void ProcessEvent() { }

        public void StartEvent()
        {
            source.Play();
        }
    }
}
