using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script
{
    public interface IEventController
    {
        GameObject GetNextEventSource();
        string GetEventType();
        void StartEvent(GameEvent gameEvent);
        void ProcessEvent(GameEvent gameEvent);
        bool GetIsProcessComplete(GameEvent gameEvent);
        bool GetIsEventComplete(GameEvent gameEvent);
        bool GetIsGameEventComplete(GameEvent gameEvent);
        void FinishEvent(GameEvent gameEvent);
        void ResetEvent(GameEvent gameEvent);
    }
}
