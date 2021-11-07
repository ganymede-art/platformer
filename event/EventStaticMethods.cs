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
        public static void DrawEventGizmo(IEventController eventController, GameObject thisEventObject, GameObject nextEventObject,
            GameObject optionalObject1 = null, Color? optionalColour1 = null, string optionalIcon1 = null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawIcon(thisEventObject.transform.position, "ev.png", true);
            if (nextEventObject != null)
                Gizmos.DrawLine(thisEventObject.transform.position, nextEventObject.transform.position);
            if (optionalObject1 != null)
            {
                Gizmos.color = optionalColour1 == null ? Color.red : optionalColour1.Value;
                Gizmos.DrawLine(thisEventObject.transform.position, optionalObject1.transform.position);
                if (optionalIcon1 != null)
                    Gizmos.DrawIcon(optionalObject1.transform.position, optionalIcon1);
            }
        }
    }
}
