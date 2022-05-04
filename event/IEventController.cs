using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script
{
    public interface IEventController
    {
        GameObject GetNextEventSource();
        string GetEventType();
        string GetEventDescription();
        void StartEvent(GameEvent gameEvent);
        void UpdateEvent(GameEvent gameEvent);
        bool GetIsUpdateComplete(GameEvent gameEvent);
        bool GetIsEventComplete(GameEvent gameEvent);
        void FinishEvent(GameEvent gameEvent);
        void ResetEvent(GameEvent gameEvent);
    }
}
