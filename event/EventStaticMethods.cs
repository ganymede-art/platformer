using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script
{
    public static class EventStaticMethods
    {
        public static void DrawEventGizmo(IEventController eventController, GameObject thisEventObject, GameObject nextEventObject)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawIcon(thisEventObject.transform.position, "giz_ev.png", true);
            if (nextEventObject != null)
                Gizmos.DrawLine(thisEventObject.transform.position, nextEventObject.transform.position);
        }
    }
}
