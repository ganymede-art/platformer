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
            GameMasterController.GetMasterController().audio_controller.PlayMusic(musicData);
        }
    }
}
