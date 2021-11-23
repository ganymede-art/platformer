using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script
{
    class EventConditionalBool : MonoBehaviour, IEventController
    {
        private IReplacerController replacerComponent;
        private object replacerRawValue;
        private bool replacerValue;
        private bool isValidConditional = false;

        [Header("Event sources.")]
        public GameObject nextEventSource;
        public GameObject conditionalEventSource = null;
        [Header("Source of comparison value.")]
        public GameObject replacerObject;
        [Header("Type of comparison.")]
        public ConditionalBoolType conditionalType;
        [Header("Value to compare against.")]
        public bool conditionalValue;


        public string GetEventDescription()
        {
            return GameConstants.EVENT_TYPE_CONDITIONAL_BOOL;
        }

        public string GetEventType() { return GameConstants.EVENT_TYPE_CONDITIONAL_BOOL; }
        public void StartEvent(GameEvent gameEvent)
        {
            replacerComponent = replacerObject.GetComponent<IReplacerController>();

            if (replacerComponent == null)
            {
                Debug.LogError("Replacer component missing.");
                return;
            }

            replacerRawValue = replacerComponent.GetReplacementValue();

            if (replacerRawValue == null)
            {
                Debug.LogError("Replacer raw value null.");
                return;
            }

            if (replacerRawValue.GetType() != typeof(bool))
            {
                Debug.LogError("Replacer raw value is not a bool.");
                return;
            }

            replacerValue = Convert.ToBoolean(replacerRawValue);
            isValidConditional = true;
        }

        public GameObject GetNextEventSource()
        {
            if (!isValidConditional)
                return nextEventSource;

            if (conditionalType == ConditionalBoolType.isEqual)
            {
                if (replacerValue == conditionalValue)
                    return conditionalEventSource;
            }
            else if(conditionalType == ConditionalBoolType.isNotEqual)
            {
                if (replacerValue != conditionalValue)
                    return conditionalEventSource;
            }

            return nextEventSource;
        }

        public bool GetIsEventComplete(GameEvent gameEvent) { return true; }
        public bool GetIsUpdateComplete(GameEvent gameEvent) { return true; }
        public void ResetEvent(GameEvent gameEvent) {}
        public void UpdateEvent(GameEvent gameEvent) {}
        public void FinishEvent(GameEvent gameEvent) { }

        private void OnDrawGizmos()
        {
            EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource,
                optionalObject1: conditionalEventSource ?? gameObject, optionalColour1: Color.green, optionalIcon1: "ev_conbool.png");
        }
    }
}
